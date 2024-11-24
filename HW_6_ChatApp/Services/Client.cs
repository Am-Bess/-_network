using System.Net;
using System.Net.Sockets;
using HW_6_ChatApp.Abstraction;
using HW_6_ChatApp.Models;

namespace HW_6_ChatApp.Services
{
    public class Client
    {
        private UdpClient udpClientClient = new UdpClient() ;
        private readonly string _adress;
        private readonly int _port;
        private readonly string _name;

        public IMessageSource messageSource;

        public bool work = true;

        public Client(IMessageSource source, string adress, int port, string name)
        {
            _adress = adress;
            _port = port;
            _name = name;
            messageSource = source;
        }

        public void ClientStart()
        {
            udpClientClient = new UdpClient(_port);
            new Thread(ClientListener).Start();
            ClientSender();
        }

        public void ClientStop()
        {
            work = false;
        }

        public void ClientListener()
        {
            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse(_adress), 12345);

            while (work)
            {
                try
                {
                    var messageReceived = messageSource.Receive(ref remoteEndPoint);
                    Console.WriteLine("Получено сообщение от: " + messageReceived.FromName);
                    Console.WriteLine(messageReceived.Text);

                    ClientConfirm(messageReceived, remoteEndPoint);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Возникло исключение: " + e);
                }
            }
        }

        public void ClientSender()
        {
            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse(_adress), 12345);
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

                    messageSource.Send(message, remoteEndPoint);
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
            messageSource.Send(message, remoteEndPoint);
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
            messageSource.Send(message, remoteEndPoint);
        }
    }
}