using CommandLine;

namespace Agent
{
    public class Options
    {
        [Option('n', "name", Required = true, HelpText = "Agent name")]
        public string Name { get; set; }

        [Option('h', "host", Required = true, HelpText = "Host address")]
        public string Host { get; set; }

        [Option('r', "remote", Required = true, HelpText = "Remote controller URL")]
        public string RemoteControllerUrl { get; set; }
    }
}
