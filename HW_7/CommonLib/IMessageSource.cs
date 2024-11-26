using ChatCommLib;

namespace CommonLib
{
    public interface IMessageSource
    {
        void Send(MessageUdp message, string clientId);
        MessageUdp Receive(ref string? clientId);
    }
}
