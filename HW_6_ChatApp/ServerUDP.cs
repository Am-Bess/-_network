using System.Net;
using HW_6_ChatApp.Model;
using HW_6_ChatApp.Abstraction;
using System.Net.Sockets;

namespace HW_6_ChatApp
{
    public class ServerUDP
    {
        private UdpClient? udpClient;
        private IPEndPoint? ip;
        private Dictionary<String, IPEndPoint> clients = new Dictionary<String, IPEndPoint>();

        IMessageSource messageSource;

        bool work = true;

        //Токен для остановки сервера
        static private CancellationTokenSource cts = new CancellationTokenSource();

        static private CancellationToken ct = cts.Token;


        public ServerUDP(IMessageSource source)
        {
            messageSource = source;
        }

        public void Work()
        {
            ip = new IPEndPoint(IPAddress.Any, 0);

            Console.WriteLine("Сервер ожидает сообщение.");

            while (work)
            {
                var message = messageSource.ReceiveMessage(ref ip);
                ProcessMessage(message);
            }
        }

        public void Stop()
        {
            work = false;
        }


        private void ProcessMessage(MessageUDP messageUdp)
        {
            Console.WriteLine($"Получено сообщение от {messageUdp?.FromName} для {messageUdp?.ToName} с командой {messageUdp?.Command}:");
            Console.WriteLine(messageUdp?.Text);

            switch (messageUdp?.Command)
            {
                case Command.Mes:
                    RelyMessage(messageUdp);
                    break;
                case Command.Reg:
                    Register(messageUdp);
                    break;
                case Command.Conf:
                    ConfirmMessageReceived(messageUdp.Id);
                    break;
            }

        }

        private void RelyMessage(MessageUDP messageUdp)
        {
            if (clients.TryGetValue(messageUdp.ToName!, out IPEndPoint? ep))
            {
                int id;

                using (ContextDB context = new ContextDB())
                {
                    var fromUser = context.Users?.FirstOrDefault((u) => u.Name == messageUdp.FromName);
                    var toUser = context.Users?.FirstOrDefault((u) => u.Name == messageUdp.ToName);
                    var messageDd = new Message()
                    {
                        Text = messageUdp.Text,
                        DateMessage = DateTime.Now,
                        IsReceived = false,
                        ToUser = toUser,
                        FromUser = fromUser,
                    };
                    context.Messages?.Add(messageDd);
                    context.SaveChanges();
                    id = messageDd.Id;
                }

                var forwardMessageJson = new MessageUDP()
                {
                    Id = id,
                    Command = Command.Mes,
                    ToName = messageUdp.ToName,
                    FromName = messageUdp.FromName,
                    Text = messageUdp.Text
                };

                messageSource.SendMessage(forwardMessageJson, ep);

                Console.WriteLine($"Message Relied, from = {messageUdp.FromName} to = {messageUdp.ToName}");
            }
            else
            {
                Console.WriteLine("Пользователь не найден.");
            }

        }

        void ConfirmMessageReceived(int? id)
        {
            Console.WriteLine("Message confirmation id=" + id);

            using (var ctx = new ContextDB())
            {
                var msg = ctx.Messages?.FirstOrDefault(x => x.Id == id);

                if (msg != null)
                {
                    msg.IsReceived = true;
                    ctx.SaveChanges();
                }
            }
        }


        private void Register(MessageUDP messageUdp)
        {
            Console.WriteLine($"Message register, Name = {messageUdp.FromName}");
            clients.Add(messageUdp.FromName!, ip!);

            using (ContextDB context = new ContextDB())
            {
                if (context.Users.Any((u) => u.Name == messageUdp.FromName))
                {
                    return;
                }
                context.Add(new User() { Name = messageUdp.FromName });
                context.SaveChanges();
            }
        }
    }
}


