using System.Net.Sockets;
using System.Net;
using System.Text;
using HW_6_ChatApp.Models;
using HW_6_ChatApp.Abstraction;

namespace HW_6_ChatApp.Services
{
    public class UdpMessageSource : IMessageSource
    {
        private readonly UdpClient _udpClient;

        public UdpMessageSource(int port = 12345)
        {
            _udpClient = new UdpClient(port);
        }

        public MessageUdp Receive(ref IPEndPoint ep)
        {
            byte[] receiveBytes = _udpClient.Receive(ref ep);
            string receivedData = Encoding.UTF8.GetString(receiveBytes);
            return MessageUdp.MessageFromJson(receivedData);
        }

        public void Send(MessageUdp message, IPEndPoint ep)
        {
            byte[] forwardBytes = Encoding.UTF8.GetBytes(message.MessageToJson());
            _udpClient.Send(forwardBytes, forwardBytes.Length, ep);
            Console.WriteLine("Сообщение отправлено.");
        }
    }
}
