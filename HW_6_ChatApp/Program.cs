using HW_6_ChatApp;

// dotnet ef migrations add initialcreate
// dotnet ef database update 

internal class Program
{
    private static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            MessageSource messageSourceServer = new MessageSource(5566);
            ServerUDP server = new ServerUDP(messageSourceServer);
            server.Work();
        }
        else if (args.Length == 1)
        {
            MessageSource messageSourceClient = new MessageSource(6655);
            ClientUDP client = new ClientUDP( messageSourceClient, "127.0.0.2", 5555, args[0]);
            client.Start();
        }
    }
}