using System.Net;
using System.Net.Sockets;
using System.Text;

namespace HW_4
{
    internal class Server
    {
        private const string serverName = "Server";
        public static Dictionary<string, IPEndPoint> clients = new Dictionary<string, IPEndPoint>();
        static private CancellationTokenSource cts = new CancellationTokenSource();

        public static async Task AcceptMsg()
        {
            IPEndPoint iPEndPoint = new IPEndPoint((IPAddress)IPAddress.Any, 0);
            UdpClient? udpClient = new UdpClient(12345);
            Console.WriteLine("Сервер ожидает сообщения отклиента");

            while (true)
            {
                try
                {
                    byte[] buffer = udpClient.Receive(ref iPEndPoint);
                    string data = Encoding.UTF8.GetString(buffer, 0, buffer.Length);

                    await Task.Run(async () =>
                     {
                         if (cts.IsCancellationRequested)  // проверяем наличие сигнала отмены задачи
                         {
                             Console.WriteLine("Операция прервана");
                             return;     //  выходим из метода и тем самым завершаем задачу
                         }
                         Message? msgFromClient = Message.FromJson(data);


                         if (msgFromClient != null)
                         {

                             Message newMsg = new Message();

                             if (msgFromClient.ToName.Equals(serverName))
                             {
                                 if (msgFromClient.Text.ToLower().Equals("reg"))
                                 {
                                     if (clients.TryAdd(msgFromClient.FromName, iPEndPoint))
                                     {
                                         newMsg = new Message(serverName, $"Пользователь добавлен {msgFromClient.FromName}");
                                     }
                                 }
                                 else if (msgFromClient.Text.ToLower().Equals("del"))
                                 {
                                     clients.Remove(msgFromClient.FromName);
                                     newMsg = new Message(serverName, $"Клиент {msgFromClient.FromName} удален");
                                 }
                                 else if (msgFromClient.Text.ToLower().Equals("list"))
                                 {
                                     StringBuilder list = new StringBuilder();
                                     foreach (var item in clients)
                                     {
                                         list.Append(item.Key + "\n");
                                     }
                                     newMsg = new Message(serverName, $"Список клиентов: {list.ToString()}");
                                 }
                             }
                             else if (msgFromClient.ToName.ToLower().Equals("all"))
                             {
                                 foreach (var item in clients)
                                 {
                                     msgFromClient.ToName = item.Key;
                                     var js = msgFromClient.ToJson();
                                     var b = Encoding.UTF8.GetBytes(js);
                                     await udpClient.SendAsync(b, item.Value);

                                     newMsg = new Message(serverName, "Сообщение отправлено всем!");
                                 }
                             }
                             else if (clients.TryGetValue(msgFromClient.ToName, out IPEndPoint? val))
                             {
                                 var js = msgFromClient.ToJson();
                                 var b = Encoding.UTF8.GetBytes(js);
                                 await udpClient.SendAsync(b, val);
                                 newMsg = new Message(serverName, $"Пользователю {msgFromClient.FromName} отправлено сообщение");
                             }
                             else
                             {
                                 newMsg = new Message(serverName, $"Пользователь {msgFromClient.FromName} не существует");
                             }
                             Console.WriteLine(msgFromClient?.ToString());
                             var json = newMsg.ToJson();
                             var bytes = Encoding.UTF8.GetBytes(json);
                             await udpClient.SendAsync(bytes, iPEndPoint);
                         }
                         else { Console.WriteLine("Сообщение не корректно"); }

                     }, cts.Token);

                }
                catch (Exception e)
                {
                    //udpClient?.Close();
                    Console.WriteLine($"Что-то пошло не так: {e.Message}");
                }
            }
        }
    }
}