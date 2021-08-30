using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Extensions.Msal;

namespace teamslink
{
    public class AuthProvider : IAuthenticationProvider
    {
        private static string[] SCOPES = {
            "User.Read",
            "OnlineMeetings.ReadWrite"
        };

        private static AuthProvider _Instance = null;

        public static async Task<AuthProvider> Get()
        {
            if (_Instance == null)
            {
                var settings = SettingsProvider.GetSettings();
                _Instance = new AuthProvider(settings.appId, settings.tenantId, SCOPES);
                await _Instance.Init();
            }
            return _Instance;
        }

        private IPublicClientApplication _msalClient;
        private string[] _scopes;
        private StorageCreationProperties _storageProperties;

        private AuthProvider(string appId, string tenantId, string[] scopes)
        {
            _scopes = scopes;

            _msalClient = PublicClientApplicationBuilder
                .Create(appId)
                .WithAuthority(AadAuthorityAudience.AzureAdMyOrg, true)
                .WithTenantId(tenantId)
                .WithRedirectUri("http://localhost")
                .Build();

            _storageProperties = new StorageCreationPropertiesBuilder(".tmg.tmp", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile))
                                    .WithLinuxKeyring(
                                        "create-teams-meeting",
                                        "default",
                                        "Access tokens used by create-teams-meeting",
                                        new KeyValuePair<string, string>("test", "test"),
                                        new KeyValuePair<string, string>("test1", "test1"))
                                    .Build();
        }

        private async Task Init()
        {

            var cacheHelper = await MsalCacheHelper.CreateAsync(_storageProperties);
            cacheHelper.RegisterCache(_msalClient.UserTokenCache);
        }

        public async Task<bool> IsLoggedIn()
        {
            var accounts = await _msalClient.GetAccountsAsync();
            return accounts.Count() > 0;
        }

        public async Task Login()
        {
            await Logout();
            await _msalClient.AcquireTokenInteractive(_scopes).ExecuteAsync();
        }

        public async Task Logout()
        {
            var accounts = await _msalClient.GetAccountsAsync();
            foreach (var account in accounts)
            {
                await _msalClient.RemoveAsync(account);
            }
        }

        public async Task<string> GetAccessToken()
        {
            var accounts = await _msalClient.GetAccountsAsync();
            if (accounts.Count() == 0)
            {
                throw new Exception("Not logged in");
            }
            var userAccount = accounts.First();

            var result = await _msalClient
                .AcquireTokenSilent(_scopes, userAccount)
                .ExecuteAsync();

            return result.AccessToken;
        }

        // This is the required function to implement IAuthenticationProvider
        // The Graph SDK will call this function each time it makes a Graph
        // call.
        public async Task AuthenticateRequestAsync(HttpRequestMessage requestMessage)
        {
            requestMessage.Headers.Authorization =
                new AuthenticationHeaderValue("bearer", await GetAccessToken());
        }
    }
}
