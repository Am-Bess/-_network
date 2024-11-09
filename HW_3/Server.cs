using System.Net;
using System.Net.Sockets;

namespace HW_3
{
    internal class Server
    {
        static private CancellationTokenSource cts = new CancellationTokenSource();
        static private CancellationToken ct = cts.Token;

        public static async Task AcceptMsg()
        {
            UdpClient udpServer = new UdpClient(12345);
            IPEndPoint iPEndPoint = new IPEndPoint((IPAddress)IPAddress.Any, 0);

            string? messText = null;

            while (!ct.IsCancellationRequested)
            {
                Console.WriteLine("Ожидаем сообщение от пользователя");

                await Task.Run(() =>
                {
                    try
                    {
                        byte[] buffer = udpServer.Receive(ref iPEndPoint);
                        Message? dataMessage = Program.DataRecieve(udpServer, iPEndPoint, buffer);
                        messText = dataMessage?.TextMessage;

                        var mess = new Message()
                        {
                            NickName = "Сервер",
                            DateMessage = DateTime.Now,
                            TextMessage = "Доставлено"
                        };

                        if (messText?.ToLower() == "exit")
                        {
                            //Токен для остановки сервера
                            Console.WriteLine("Сервер ОФФ!");
                            cts.Cancel();
                        }

                        Program.DataSend(mess, udpServer, iPEndPoint);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                });
            }

        }
    }
}