
using HW_6_ChatApp.Abstraction;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace HW_6_ChatApp
{
    public class MessageSource : IMessageSource
    {
        private UdpClient udpClient;
        public MessageSource(int port)
        {
            udpClient = new UdpClient(port);
        }
        public MessageUDP ReceiveMessage(ref IPEndPoint endPoint)
        {
            byte[] receiveBytes = udpClient.Receive(ref endPoint);
            string receivedData = Encoding.ASCII.GetString(receiveBytes);

            return MessageUDP.FromJson(receivedData);
        }
        public void SendMessage(MessageUDP message, IPEndPoint endPoint)
        {
            string json = message.ToJson();
            byte[] buffer = Encoding.ASCII.GetBytes(json);
            udpClient.Send(buffer, endPoint);
        }
    }
}
