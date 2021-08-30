using System.CommandLine;

namespace teamslink
{
    public class Program
    {
        static int Main(string[] args)
        {
            // Create a root command with some options
            var rootCommand = new RootCommand("Teams Meeting Generator (tmg). Make sure to read the installation instructions, especially regarding the configuration files required before using any subcommand.");
            rootCommand.AddCommand(new LoginCommand());
            rootCommand.AddCommand(new LogoutCommand());
            rootCommand.AddCommand(new GenerateCommand());

            // Parse the incoming args and invoke the handler
            return rootCommand.InvokeAsync(args).Result;
        }
    }
}
