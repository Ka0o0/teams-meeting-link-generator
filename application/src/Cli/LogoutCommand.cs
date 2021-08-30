using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace teamslink
{
    class LogoutCommand : Command, ICommandHandler
    {
        private static string NAME = "logout";
        private static string DESCRIPTION = 
@"Remove all access tokens from the system";


        public LogoutCommand() : base(NAME, DESCRIPTION)
        {
            this.Handler = this;
        }

        public async Task<int> InvokeAsync(InvocationContext context)
        {
            await AuthProvider.Get().Result.Logout();
            return 0;
        }
    }
}
