using System.Net;
using System.Net.Sockets;
using System.Text;

namespace HW_2
{
    public class Client
    {
        public static void SendMsg(string nikname)
        {
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);
            UdpClient udpClient = new UdpClient();

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
                    
                    var msg = new Message(nikname, text);
                    var json = msg.ToJson();
                    var data = Encoding.UTF8.GetBytes(json);
                    udpClient?.Send(data, iPEndPoint);

                    byte[]? buffer = udpClient?.Receive(ref iPEndPoint);
                    string str1 = Encoding.UTF8.GetString(buffer);
                    var somemessage = Message.FromJson(str1);
                    Console.WriteLine(somemessage?.ToString());
                    
                    if (text == "Exit") {
                        udpClient?.Close();
                        System.Console.WriteLine("Клиент ОФФ!");
                        return;
                    }
                }
                catch (Exception e)
                {
                    udpClient?.Close();
                    Console.WriteLine($"Ошибка: {e.Message}");
                }
            }
        } 
    }
}