using ChatNetwork;
using ChatApp.Services;

// dotnet ef migrations add initial1.0
// dotnet ef database update 

namespace ChatApp
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                var s = new Server(new UdpMessageSourceServer());
                s.Work();
            }
            else if (args.Length == 1)
            {
                var c = new Client(args[0], 12345, "127.0.0.1");
                c.Start();
            }
            else
            {
                Console.WriteLine("Чтобы запустить клиента введите% ник, как параметры для запуска приложения");
            }
        }
    }
}
