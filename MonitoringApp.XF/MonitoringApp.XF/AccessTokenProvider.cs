using Capital.GSG.FX.Data.Core.WebApi;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Threading;
using Microsoft.Extensions.Logging;
using Capital.GSG.FX.Utils.Core.Logging;
using System.Diagnostics;

namespace MonitoringApp.XF
{
    internal class AccessTokenProvider : IAccessTokenProvider
    {
        private readonly ILogger logger = GSGLoggerFactory.Instance.CreateLogger<AccessTokenProvider>();

        private readonly string clientId;
        private readonly string commonAuthority;
        private readonly string backendAppIdUri;
        private readonly string returnUri;

        public AccessTokenProvider()
        {
            clientId = Constants.ClientId;
            commonAuthority = Constants.CommonAuthority;
            backendAppIdUri = Constants.BackendAppIdUri;
            returnUri = Constants.ReturnUri;
        }

        public async Task<string> GetAccessToken(CancellationToken ct = default(CancellationToken))
        {
            try
            {
                ct.ThrowIfCancellationRequested();

                IAuthenticator authenticator = DependencyService.Get<IAuthenticator>();

                return (await authenticator.Authenticate(commonAuthority, backendAppIdUri, clientId, returnUri))?.AccessToken;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to retrieve access token: {ex.Message}");
                return null;
            }
        }
    }
}
