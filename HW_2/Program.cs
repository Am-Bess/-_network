namespace HW_2
{
    internal class Program
    {

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Server.AcceptMsg();
            }
            else
            {
                new Thread(() =>
                {
                    Client.SendMsg(args[0]);
                }).Start();
            }
        }
    }
}
