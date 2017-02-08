using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Xamarin.Forms;

[assembly: Dependency(typeof(MonitoringApp.XF.Droid.Authenticator))]
namespace MonitoringApp.XF.Droid
{
    class Authenticator : IAuthenticator
    {
        public async Task<AuthenticationResult> Authenticate(string authority, string resource, string clientId, string returnUri)
        {
            var authContext = new AuthenticationContext(authority);

            if (authContext.TokenCache.ReadItems().Any())
                authContext = new AuthenticationContext(authContext.TokenCache.ReadItems().First().Authority);

            var uri = new Uri(returnUri);
            var platformParams = new PlatformParameters((Activity)Forms.Context);

            var authResult = await authContext.AcquireTokenAsync(resource, clientId, uri, platformParams);
            return authResult;
        }
    }
}