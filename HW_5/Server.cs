using System.Net.Sockets;
using System.Net;
using System.Text;
using DBTest.Models;

namespace DBTest
{
    public class Server
    {
        public static Dictionary<String, IPEndPoint> clients = new Dictionary<string, IPEndPoint>();
        static private CancellationTokenSource cts = new CancellationTokenSource();
        static private UdpClient? udpClient;

        static Task Register(string name, IPEndPoint fromep)
        {
            Console.WriteLine("Message Register, name = " + name);
            clients.Add(name, fromep);

            using (var ctx = new Context())
            {
                if (ctx.Users?.FirstOrDefault(x => x.Name == name) != null) return Task.CompletedTask;

                ctx.Add(new Users { Name = name });

                ctx.SaveChanges();
            }
            return Task.CompletedTask;
        }

        static Task ConfirmMessageReceived(int? id)
        {
            Console.WriteLine("Message confirmation id=" + id);

            using (var ctx = new Context())
            {
                var msg = ctx.Messages?.FirstOrDefault(x => x.Id == id);

                if (msg != null)
                {
                    msg.Received = true;
                    ctx.SaveChanges();
                }
            }
            return Task.CompletedTask;
        }

        static async Task RelyMessageAsync(MessageUDP message)
        {
            int? id = null;
            if (clients.TryGetValue(message.ToName, out IPEndPoint? ep))
            {
                using (var ctx = new Context())
                {
                    var fromUser = ctx.Users?.First(x => x.Name == message.FromName);
                    var toUser = ctx.Users?.First(x => x.Name == message.ToName);
                    var msg = new Messages { FromUser = fromUser, ToUser = toUser, Received = false, Text = message.Text };
                    ctx.Messages?.Add(msg);

                    ctx.SaveChanges();

                    id = msg.Id;

                    var forwardMessageJson = new MessageUDP() { Id = id, Command = Command.Mes, ToName = message.ToName, FromName = message.FromName, Text = message.Text }.ToJson();
                    byte[] forwardBytes = Encoding.ASCII.GetBytes(forwardMessageJson);

                    await udpClient.SendAsync(forwardBytes, forwardBytes.Length, ep);
                    Console.WriteLine($"Message Relied, from = {message.FromName} to = {message.ToName}");
                }
            }
            else
            {
                Console.WriteLine("Пользователь не найден.");
            }
        }

        static async Task ProcessMessageAsync(MessageUDP message, IPEndPoint fromep)
        {
            if (message.Command == Command.Reg)
            {
                var name = string.Empty;
                if (!string.IsNullOrEmpty(message.FromName))
                    name = message.FromName;
                else if (!string.IsNullOrEmpty(message.ToName))
                    name = message.ToName;
                await Register(name, new IPEndPoint(fromep.Address, fromep.Port));
            }
            if (message.Command == Command.Conf)
            {
                Console.WriteLine("Confirmation receiver");
                await ConfirmMessageReceived(message.Id);
            }
            if (message.Command == Command.Mes)
            {
                Console.WriteLine($"Получено сообщение от {message.FromName} для {message.ToName} с командой {message.Command}:");
                Console.WriteLine(message.Text);
                await RelyMessageAsync(message);
            }
        }

        public static void Work()
        {
            IPEndPoint remoteEndPoint;

            udpClient = new UdpClient(12345);
            remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

            Console.WriteLine("UDP Клиент ожидает сообщений...");

            while (!cts.IsCancellationRequested)
            {
                try
                {
                    byte[] receiveBytes = udpClient.Receive(ref remoteEndPoint);
                    string receivedData = Encoding.ASCII.GetString(receiveBytes);

                    Console.WriteLine(receivedData);
                    var serverTask = Task.Run(async () =>
                    {
                        MessageUDP? message = MessageUDP.FromJson(receivedData);

                        await ProcessMessageAsync(message, remoteEndPoint);

                        if (!string.IsNullOrEmpty(message.Text) && message.Text.ToLower() == "exit")
                        {
                            cts.Cancel();
                        }
                        if (cts.IsCancellationRequested)
                        {
                            Console.WriteLine("Операция прервана");
                            return;
                        }
                    }, cts.Token);

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка при обработке сообщения: " + ex.Message);
                }
            }
        }
    }
}

