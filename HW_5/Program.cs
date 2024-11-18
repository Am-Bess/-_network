using DBTest;

class Program
{
    public static async Task Main(string[] args)
    {
        if (args.Length == 0)
        {
            Server.Work();
        }
        else
        {
            await Client.ClientSendlerAsync(args[0]);
        }
    }
}