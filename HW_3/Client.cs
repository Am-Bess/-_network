using System.Net;
using System.Net.Sockets;
using System.Text;

namespace HW_3
{
    internal class Client
    {
        public static async Task SendMsg(string nikname)
        {
            UdpClient udpClient = new UdpClient();
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);
            string? message = string.Empty;

            var mess = new Message()
            {
                NickName = nikname,
                DateMessage = DateTime.Now,
                TextMessage = message
            };

            while (message?.ToLower() != "exit")
            {
                Console.Write("Введите текст ('exit' -> выход): ");
                message = Console.ReadLine();
                mess.TextMessage = message;

                await Task.Run(() =>
                {
                    try
                    {
                        Program.DataSend(mess, udpClient, iPEndPoint);
                        var buffer = udpClient.Receive(ref iPEndPoint);
                        Message? dataMessage = Program.DataRecieve(udpClient, iPEndPoint, buffer);
                        string? messText = string.Empty;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                });
            }
            Console.WriteLine("Клиент ОФФ!");
        }
    }
}