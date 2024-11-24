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
            UdpMessageSource messageSourceServer = new UdpMessageSource(12345);
            Server server = new Server(messageSourceServer);
            server.Work();
        }
        else if (args.Length == 1)
        {
            UdpMessageSource messageSourceClient = new UdpMessageSource(12345);
            Client client = new Client(messageSourceClient, "127.0.0.1", 1234, args[0]);
            client.ClientStart();
        }
    }
}