using System.Net;

namespace DBTest
{
    public class Client
    {
        private static IPEndPoint _iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);

        public static bool isWork = true;
        public static async Task ClientSendlerAsync(string name)
        {
            var msgFromName = new MessageUDP()
            {
                Command = Command.Reg,
                FromName = name,
                Text = string.Empty
            };
            await MessageSource.SendAsync(msgFromName, _iPEndPoint);
            while (isWork)
            {
                try
                {
                    Console.Write("Введите имя получателя: ");
                    var toName = Console.ReadLine();
                    if (String.IsNullOrEmpty(toName))
                    {
                        Console.Write(" Вы не ввели имя получателя");
                        continue;
                    }
                    MessageUDP? msgToName = new MessageUDP()
                    {
                        Command = Command.Reg,
                        ToName = toName,
                        Text = string.Empty
                    };
                    await MessageSource.SendAsync(msgToName, _iPEndPoint);

                    Console.Write("Введите сообщение (команда, ник, текст): ");
                    string? text = Console.ReadLine();
                    if (String.IsNullOrEmpty(text) || text.ToLower() == "exit")
                    {
                        isWork = false;
                        // break;
                    }
                    MessageUDP? msgWithText = new MessageUDP()
                    {
                        Command = Command.Mes,
                        Text = text,
                        FromName = name,
                        ToName = toName
                    };
                    await MessageSource.SendAsync(msgWithText, _iPEndPoint);

                    MessageUDP? msg = await MessageSource.ReceiveAsync();
                    Console.WriteLine(msg?.ToString());

                    msg.Command = Command.Conf;
                    await MessageSource.SendAsync(msg, _iPEndPoint);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Что-то пошло не так: {e.Message}");
                }
            }
        }
    }
}
