using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace teamslink
{
    class LoginCommand : Command
    {
        private static string NAME = "login";
        private static string DESCRIPTION =
@"Use this command to login to a microsoft account.
This command will open the system's default web browser to perform the oAuth2 login flow.
Upon success, it will store the access token in the system's default keystore, e.g. keyring or ksecret.
If another account is already logged in a logout is automatically performed.";


        public LoginCommand() : base(NAME, DESCRIPTION)
        {
            this.SetHandler(this.InvokeAsync);
        }

        public async Task<int> InvokeAsync(InvocationContext context)
        {
            await AuthProvider.Get().Result.Login();
            return 0;
        }
    }
}
