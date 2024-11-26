using ChatCommLib;
using CommonLib;
using NetMQ;
using NetMQ.Sockets;
using System.Text;

namespace ChatNetwork
{
    public class UdpMessageSourceServer : IMessageSource
    {
        private readonly RouterSocket _routerSocket = new();

        public UdpMessageSourceServer() 
        {
            _routerSocket.Bind($"tcp://*:{12345}");
        }

        public MessageUdp Receive(ref string? clientID)
        {
            string messageReceived = _routerSocket.ReceiveFrameString();    // получаем само сообщени

            return MessageUdp.FromJson(messageReceived) ?? new MessageUdp();
        }

        public void Send(MessageUdp message, string clientId)
        {
            // отправляем идентификатор клиента
            _routerSocket.SendMoreFrame(clientId);

            // отправляем разделитель (обычно пустая строка)
            _routerSocket.SendMoreFrame(""); // Пустой фрейм для разделения метаданных

            // отправляем само сообщение
            _routerSocket.SendFrame(Encoding.UTF8.GetBytes(message.ToJson()));
        }
    }
}