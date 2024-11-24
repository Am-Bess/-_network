using HW_6_ChatApp.Abstraction;
using System.Net;
using System.Net.Sockets;

namespace HW_6_ChatApp
{
    public class ClientUDP
    {
        UdpClient udpClientClient = new UdpClient();
        private readonly string _adress;
        private readonly int _port;
        private readonly string _name;

        IMessageSource messageSource;

        bool work = true;

        public ClientUDP(IMessageSource source, string adress, int port, string name)
        {
            _adress = adress;
            _port = port;
            _name = name;
            messageSource = source;
        }

        public void Start()
        {
            udpClientClient = new UdpClient(_port);
            new Thread(() => Listener()).Start();
            Sender();
        }

        public void Stop()
        {
            work = false;
        }

        public void Listener()
        {
            while (work)
            {
                IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse(_adress), 12345);
                try
                {
                    var message = messageSource.ReceiveMessage(ref remoteEndPoint);
                    Console.WriteLine("Получено сообщение от: " + message.FromName);
                    Console.WriteLine(message.Text);
                    Confirmation();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Возникло исключение: " + e);
                }
            }
        }

        public void Sender()
        {
            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse(_adress), 12345);

            while (work)
            {
                try
                {
                    Console.WriteLine("Ожидается ввод сообщения:");
                    var text = Console.ReadLine();
                    Console.WriteLine("Введите имя получателя:");
                    var toName = Console.ReadLine();

                    MessageUDP message = new MessageUDP()
                    {
                        Command = Command.Mes,
                        ToName = toName,
                        FromName = _name,
                        Text = text
                    };

                    messageSource.SendMessage(message, remoteEndPoint);
                    Console.WriteLine("Сообщение отправлено.");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Ошибка при обработке сообщения : " + e);
                }
            }

        }
        public void Register()
        {
            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse(_adress), 12345);

            MessageUDP message = new MessageUDP()
            {
                Command = Command.Reg,
                ToName = null,
                FromName = _name,
                Text = null
            };

            messageSource.SendMessage(message, remoteEndPoint);
        }

        public void Confirmation()
        {
            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse(_adress), 12345);

            MessageUDP message = new MessageUDP()
            {
                Command = Command.Conf,
                ToName = null,
                FromName = _name,
                Text = null
            };

            messageSource.SendMessage(message, remoteEndPoint);
        }
    }
}