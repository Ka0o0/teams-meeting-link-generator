using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace teamslink
{
    class GenerateCommand : Command
    {
        private static string NAME = "generate";
        private static string DESCRIPTION =
@"Generate a meeting with link. You can specify a subject as well. The link will be printed out.";


        public GenerateCommand() : base(NAME, DESCRIPTION)
        {
            var subjectOptions = new Option<string>(new[] { "--subject", "-s" }, () => "Meeting", "Specifies the subject for the meeting. This will be shown e.g. if you click on the link in the browser or before joining the meeting.");
            this.AddOption(subjectOptions);
            this.Handler = CommandHandler.Create<string>(this.InvokeAsync);
        }

        private async Task<int> InvokeAsync(string subject)
        {
            var authProvider = await AuthProvider.Get();
            var graphHelper = new GraphHelper(authProvider);
            var link = await graphHelper.CreateMeeting(subject);

            Console.WriteLine(link);
            return 0;
        }
    }
}
