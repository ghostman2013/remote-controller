using CommandLine;
using System;
using System.Threading.Tasks;

namespace Agent
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed(options => Start(options));
        }

        static void Start(Options options)
        {
            var client = new Client(options.RemoteControllerUrl, options.Host);

            var task = Task.Run(async () => await client.Connect());
            task.Wait();

            if (task.Result)
            {
                var agent = new Agent(options.Name);
                var server = new Server(options.Host, agent);
                server.Start();
            }
            else
            {
                Console.WriteLine("Can't connect.");
            }
        }
    }
}
