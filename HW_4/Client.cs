using System.Net;
using System.Net.Sockets;
using System.Text;

namespace HW_4
{
    internal class Client
    {
        public static async Task ClientSendler(string nikname)
        {
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);
            UdpClient udpClient = new UdpClient();
            while (true)
            {
                try
                {
                    Console.WriteLine("Введите имя получателя");
                    var toName = Console.ReadLine();
                    if (String.IsNullOrEmpty(toName))
                    {
                        Console.WriteLine("Вы не ввели имя получателя");
                        continue;
                    }
                    Console.WriteLine("Введите сообщение");
                    var text = Console.ReadLine();
                    if (String.IsNullOrEmpty(text) || text.ToLower() == "exit")
                    {
                        break;
                    }
                    var msg = new Message(nikname, text);
                    msg.ToName = toName;
                    var json = msg.ToJson();
                    var data = Encoding.UTF8.GetBytes(json);
                    var t = await udpClient.SendAsync(data, iPEndPoint);

                    var q = await udpClient.ReceiveAsync();
                    byte[] buffer = q.Buffer;//udpClient.Receive(ref iPEndPoint);
                    string str1 = Encoding.UTF8.GetString(buffer);
                    var somemessage = Message.FromJson(str1);
                    Console.WriteLine(somemessage?.ToString());
                }
                catch (Exception e)
                {
                    udpClient?.Close();
                    Console.WriteLine($"Что-то пошло не так: {e.Message}");
                }
            }
        }

        public static async Task ClientListener()
        {
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);
            UdpClient udpClient = new UdpClient();
            while (true)
            {
                try
                {
                    var q = await udpClient.ReceiveAsync();
                    byte[] buffer = q.Buffer;//udpClient.Receive(ref iPEndPoint);
                    string str1 = Encoding.UTF8.GetString(buffer);
                    var somemessage = Message.FromJson(str1);
                    Console.WriteLine(somemessage?.ToString());
                }
                catch (Exception e)
                {
                    udpClient?.Close();
                    Console.WriteLine($"Что-то пошло не так: {e.Message}");
                }
            }
        }

        public static async Task SendMsg(string nikname)
        {
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);
            UdpClient udpClient = new UdpClient();
            while (true)
            {
                try
                {
                    Console.WriteLine("Введите имя получателя");
                    var toName = Console.ReadLine();
                    if (String.IsNullOrEmpty(toName)){
                        Console.WriteLine("Вы не ввели имя получателя");
                        continue;
                    }
                    Console.WriteLine("Введите сообщение");
                    var text = Console.ReadLine();
                    if (String.IsNullOrEmpty(text) || text.ToLower() == "exit")
                    {
                        break;
                    }
                    var msg = new Message(nikname, text);
                    msg.ToName = toName;
                    var json = msg.ToJson();
                    var data = Encoding.UTF8.GetBytes(json);
                    var t = await udpClient.SendAsync(data, iPEndPoint);

                    var q = await udpClient.ReceiveAsync();
                    byte[] buffer = q.Buffer;//udpClient.Receive(ref iPEndPoint);
                    string str1 = Encoding.UTF8.GetString(buffer);
                    var somemessage = Message.FromJson(str1);
                    Console.WriteLine(somemessage?.ToString());
                }
                catch (Exception e)
                {
                    udpClient?.Close();
                    Console.WriteLine($"Что-то пошло не так: {e.Message}");
                }
            }
        }
    }
}