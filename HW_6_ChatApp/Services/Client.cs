using System.Net;
using System.Net.Sockets;
using HW_6_ChatApp.Abstraction;
using HW_6_ChatApp.Models;

namespace HW_6_ChatApp.Services
{
    public class Client
    {
        private readonly string _name;

        private IMessageSource _messageSource;
        private readonly IPEndPoint _IPEndPoint;   

        public bool work = true;

        public Client(string name, string ipAdress, int port)
        {
            _messageSource = new UdpMessageSource(port);
            _IPEndPoint = new IPEndPoint(IPAddress.Parse(ipAdress), 12345);
            _name = name;
        }

        public void StartClient()
        {
            new Thread(ClientListener).Start();
            ClientSender();            
        }

        public void ClientStop()
        {
            work = false;
        }

        public void ClientListener()
        {
            Console.WriteLine("UDP Клиент запущен...");
            IPEndPoint endPoint = new IPEndPoint(_IPEndPoint.Address, _IPEndPoint.Port);

            while (work)
            {
                try
                {
                    var messageReceived = _messageSource.Receive(ref endPoint);
                    if (messageReceived != null)
                    {
                    Console.WriteLine("Получено сообщение от: " + messageReceived.FromName);
                    Console.WriteLine(messageReceived.Text);

                    ClientConfirm(messageReceived, endPoint);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Возникло исключение: " + e);
                }
            }
        }

        public void ClientSender()
        {
            IPEndPoint remoteEndPoint = new IPEndPoint(_IPEndPoint.Address, _IPEndPoint.Port);
            ClientRegister(remoteEndPoint);

            while (work)
            {
                try
                {
                    Console.WriteLine("UDP Клиент ожидает ввода сообщения");
                    Console.WriteLine("Ожидается ввод сообщения:");
                    var text = Console.ReadLine();
                    Console.WriteLine("Введите имя получателя:");
                    var toName = Console.ReadLine();

                    MessageUdp message = new MessageUdp()
                    {
                        Command = Command.Message,
                        ToName = toName,
                        FromName = _name,
                        Text = text
                    };

                    _messageSource.Send(message, remoteEndPoint);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Ошибка при обработке сообщения : " + e);
                }

            }

        }
        public void ClientRegister(IPEndPoint remoteEndPoint)
        {
            MessageUdp message = new MessageUdp()
            {
                Command = Command.Register,
                ToName = null,
                FromName = _name,
                Text = null
            };
            _messageSource.Send(message, remoteEndPoint);
        }

        public void ClientConfirm(MessageUdp msg, IPEndPoint remoteEndPoint)
        {
            MessageUdp message = new MessageUdp()
            {
                Command = Command.Confirmation,
                ToName = null,
                FromName = _name,
                Text = null
            };
            _messageSource.Send(message, remoteEndPoint);
        }
    }
}