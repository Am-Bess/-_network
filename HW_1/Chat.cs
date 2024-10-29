using System.Net;
using System.Net.Sockets;
using System.Text;

namespace HW_1
{
    public class Chat
    {
        private readonly UdpClient uclient;
        public Chat(string ip, int port)
        {
            this.uclient = new UdpClient();
            uclient.Connect(ip, port);
        }

          public static void Server()
        {
            IPEndPoint LocalEP = new IPEndPoint(IPAddress.Any, 0);
            UdpClient uclient = new UdpClient(12345);
            Console.WriteLine("Сервер ожидает сообщения от клиента");

            while (true)
            {
                try
                {
                    byte[]? buffer = uclient?.Receive(ref LocalEP);
                    string str = Encoding.UTF8.GetString(buffer);
                    Message? somemessage = Message.FromJson(str);
                    
                    if (somemessage != null)
                    {
                        Console.WriteLine(somemessage.ToString());

                        Message newMsg = new Message("Server", "Сообщение получено");
                        string json = newMsg.ToJson();
                        byte[] bytes = Encoding.UTF8.GetBytes(json);
                        uclient?.Send(bytes, LocalEP);
                    }
                    else { Console.WriteLine("Сообщение не корректно"); }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Ошибка: {e.Message}");
                }
            }
        }

        public static void Client(string nikname)
        {
            IPEndPoint? LocalEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);
            var uclient = new UdpClient();
            Console.Write("Введите имя: ");
            string? nik = Console.ReadLine();

            while (true)
            {
                try
                {
                    Console.Write("Введите сообщение: ");
                    var text = Console.ReadLine();
                    if (String.IsNullOrEmpty(text))
                    {
                        break;
                    }
                    Message newMsg = new Message(nik, text);
                    string json = newMsg.ToJson();
                    byte[] bytes = Encoding.UTF8.GetBytes(json);
                    uclient?.Send(bytes, LocalEP);

                    byte[]? buffer = uclient?.Receive(ref LocalEP);
                    string str = Encoding.UTF8.GetString(buffer);
                    Message? somemessage = Message.FromJson(str);
                    Console.WriteLine(somemessage?.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Ошибка: {e.Message}");
                }
            }
        }
    }
}