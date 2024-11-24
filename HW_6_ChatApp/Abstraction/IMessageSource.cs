using HW_6_ChatApp.Models;
using System.Net;


namespace HW_6_ChatApp.Abstraction
{
    public interface IMessageSource
    {
        void Send(MessageUdp message, IPEndPoint ep);
        MessageUdp Receive(ref IPEndPoint ep);
    }
}
