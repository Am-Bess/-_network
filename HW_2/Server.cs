
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace HW_2
{
    public class Server
    {
        public static void AcceptMsg()
        {
            IPEndPoint iPEndPoint = new IPEndPoint((IPAddress)IPAddress.Any, 0);
            UdpClient udpClient = new UdpClient(12345);
            Console.WriteLine("Сервер ожидает сообщения от клиента");

            while (true)
            {
                string text = "";
                try
                {
                    byte[]? buffer = udpClient?.Receive(ref iPEndPoint);
                    string data = Encoding.UTF8.GetString(buffer);
                    Message? msg = Message.FromJson(data);
 
                    Thread tr = new Thread(() =>
                    {
                        if (msg != null)
                        {
                            text = msg.Text;

                            Console.WriteLine(msg?.ToString());
                            var newMsg = new Message("Server", "Доставлено!");
                            var json = newMsg.ToJson();
                            var bytes = Encoding.UTF8.GetBytes(json);
                            udpClient?.Send(bytes, iPEndPoint);
                        }
                        else { Console.WriteLine("Сообщение не корректно"); }
                    });
                    tr.Start();
                    tr.Join();
                    
                    if (text == "Exit")
                    {
                        tr.Interrupt();
                        udpClient?.Close();
                        Console.WriteLine("Сервер ОФФ!");
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