using ChatCommLib;
using ChatNetwork;
using CommonLib;

namespace ChatApp.Services
{
    public class Client
    {
        private readonly string _name;

        private readonly IMessageSourceClient client;

        public Client(string name, int port, string ipAdress)
        {
            _name = name;
            client = new UdpMessageSourceClient(port, ipAdress);
        }

        void ClientListener()
        {
            while (true)
            {
                try
                {
                    var messageReceived = client.Receive();
                    Console.WriteLine("Message recieved from" + messageReceived.FromName + " : " + messageReceived.Text);
                    Confirm(messageReceived);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
        void Confirm(MessageUdp message)
        {
            var messageConfirm = new MessageUdp
            {
                FromName = _name,
                Text = null,
                ToName = null,
                Id = message.Id,
                Command = Command.Conf
            };
            client.Send(messageConfirm);
        }

        void Register()
        {
            var message = new MessageUdp { FromName = _name, Text = null, ToName = null, Command = Command.Reg };

            client.Send(message);
        }

        void ClientSender()
        {
            Console.WriteLine("UDP Клиент запущен!");
            Register();

            while (true)
            {
                try
                {                    
                    Console.WriteLine("Ожидается ввод сообщения:");
                    var msg = Console.ReadLine();
                    Console.WriteLine("Введите имя получателя:");
                    var nikName = Console.ReadLine();

                    var message = new MessageUdp() { Command = Command.Mes, FromName = _name, ToName = nikName, Text = msg };

                    client.Send(message);
                    Console.WriteLine("Сообщение отправлено.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }

        public void Start()
        {
            new Thread(ClientListener).Start();
            ClientSender();
        }
    }
}
