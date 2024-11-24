using HW_6_ChatApp.Services;


namespace HW_6_ChatApp;

// dotnet ef migrations add initialcreate
// dotnet ef database update 

internal class Program
{
    private static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Server server = new Server(new UdpMessageSource());
            server.Work();
        }
        else if (args.Length == 1)
        {
            Client client = new Client(args[0], "127.0.0.1", 8080);
            client.StartClient();
        }
    }
}