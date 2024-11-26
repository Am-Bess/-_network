using ChatCommLib;
using ChatApp.Models;
using CommonLib;

namespace ChatApp.Services
{
    public class Server
    {
        private readonly Dictionary<String, string> clients = new();
        private readonly IMessageSource sourceMessage;

        bool work = true;
        public Server(IMessageSource source)
        {
            sourceMessage = source;
        }

        void Register(MessageUdp message, string clientId)
        {
            clients.Add(message.FromName ?? "Unknown", clientId);

            using (var ctx = new ChatDbContext())
            {
                if (ctx.Users?.FirstOrDefault(x => x.Name == message.FromName) != null)
                {
                    return;
                }
                else
                {
                    ctx.Add(new Users { Name = message.FromName ?? "Unknown" });
                    ctx.SaveChanges();
                }
            }
        }

        void ConfirmMessageReceived(int? id)
        {
            using (var ctx = new ChatDbContext())
            {
                var msg = ctx.Messages?.FirstOrDefault(x => x.Id == id);
                if (msg != null)
                {
                    msg.IsReceived = true;
                    ctx.SaveChanges();
                }
            }
        }

        void ReplyMessage(MessageUdp message)
        {
            int? id = null;
            if (clients.TryGetValue(message.ToName ?? "Unknown", out string? clientId))
            {
                using (var ctx = new ChatDbContext())
                {
                    var fromUser = ctx.Users?.First(x => x.Name == message.FromName);
                    var toUser = ctx.Users?.First(x => x.Name == message.ToName);
                    var mesg = new Messages { FromUserId = fromUser?.Id, ToUserId = toUser?.Id, IsReceived = false, Text = message.Text };
                    ctx.Messages?.Add(mesg);
                    ctx.SaveChanges();
                    id = mesg.Id;
                }
                var forwardMessage = new MessageUdp() { Id = id, Command = Command.Mes, ToName = message.ToName, FromName = message.FromName, Text = message.Text };
                sourceMessage.Send(forwardMessage, clientId);
                Console.WriteLine($"Message Relied, from = {message.FromName} to = {message.ToName}");
            }
            else { Console.WriteLine("Пользователь не найден."); }
        }

        void ProcessMessage(MessageUdp message, string fromid)
        {
            Console.WriteLine($"Получено сообщение от {message.FromName} для {message.ToName} с командой {message.Command}:");
            Console.WriteLine(message.Text);
            if (message.Command == Command.Reg)
            {
                Register(message, fromid);
            }
            else if (message.Command == Command.Conf)
            {
                Console.WriteLine("Confirmation receiver");
                ConfirmMessageReceived(message.Id);
            }
            if (message.Command == Command.Mes)
            {
                ReplyMessage(message);
            }
        }

        public void Stop()
        {
            work = false;
        }

        public void Work()
        {
            Console.WriteLine("UDP Сервер ожидает сообщений...");
            while (work)
            {
                try
                {
                    string? clientId = null;
                    var message = sourceMessage.Receive(ref clientId);
                    if (message == null || clientId == null) return;
                    else
                    {
                        ProcessMessage(message, clientId);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка при обработке сообщения: \n\n" + ex);
                }
            }
        }
    }
}
