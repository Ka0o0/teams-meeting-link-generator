using System;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Kiota.Abstractions.Authentication;

namespace teamslink
{
    public class GraphHelper
    {
        private GraphServiceClient graphClient;
        public GraphHelper(IAuthenticationProvider authProvider)
        {
            graphClient = new GraphServiceClient(authProvider);
        }

        public async Task<User> GetMeAsync()
        {
            try
            {
                // GET /me
                return await graphClient.Me
                    .GetAsync();
            }
            catch (ServiceException ex)
            {
                Console.WriteLine($"Error getting signed-in user: {ex.Message}");
                return null;
            }
        }

        public async Task<string> CreateMeeting(string subject)
        {
            var meeting = new OnlineMeeting() { Subject = subject };
            var result = await graphClient.Me.OnlineMeetings.PostAsync(meeting);
            return result.JoinWebUrl;
        }
    }
}
