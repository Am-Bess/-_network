using System.Net.Sockets;
using System.Net;
using System.Text;

namespace HW_3
{
    internal class Program
    {
        public static void DataSend(Message mess, UdpClient udpClient, IPEndPoint iPEndPoint)
        {
            string data = mess.MessageToJson();
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            udpClient.Send(buffer, iPEndPoint);
        }

        public static Message? DataRecieve(UdpClient udpClient, IPEndPoint iPEndPoint, byte[] buffer)
        {
            var data = Encoding.ASCII.GetString(buffer);
            var dataMessage = Message.MessageFromJson(data);
            Console.WriteLine(dataMessage?.ToString());
            return dataMessage;
        }

        static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                await Task.Run(() => Server.AcceptMsg());
            }
            else
            {
                await Task.Run(() => Client.SendMsg(args[0]));
            }
        }
    }
}