using System.Net;

namespace HW_6_ChatApp.Abstraction
{
    public interface IMessageSource
    {
        void SendMessage(MessageUDP message, IPEndPoint endPoint);
        MessageUDP ReceiveMessage(ref IPEndPoint endPoint);
    }
}
