namespace HW_4
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                await Server.AcceptMsg();
            }
            else
            {
                await Client.ClientSendler(args[0]);
                await Client.ClientListener();
            }
        }
    }
}
