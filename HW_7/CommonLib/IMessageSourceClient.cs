using ChatCommLib;

namespace CommonLib
{
    public interface IMessageSourceClient
    {
        public void Send(MessageUdp message);
        public MessageUdp Receive();
    }
}
