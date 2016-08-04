using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Capital.GSG.FX.Monitor.App.XF;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace Capital.GSG.FX.Monitor.App.XF.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IAuthenticate
    {
        MobileServiceUser user;

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();

            // IMPORTANT: uncomment this code to enable sync on Xamarin.iOS
            // For more information, see: http://go.microsoft.com/fwlink/?LinkId=620342
            //SQLitePCL.CurrentPlatform.Init();

            App.Init(this);

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }

        public async Task<bool> AuthenticateAsync()
        {
            try
            {
                if (user == null)
                {
                    user = await TodoItemManager.DefaultManager.CurrentClient.LoginAsync(UIApplication.SharedApplication.KeyWindow.RootViewController, MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory);

                    if (user != null)
                    {
                        var authAlert = new UIAlertView("Authentication", $"You are now logged in - {user.UserId}", null, "OK", null);
                        authAlert.Show();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                var authAlert = new UIAlertView("Authentication failed", ex.Message, null, "OK", null);
                authAlert.Show();

                return false;
            }
        }

        public async Task<bool> LogoutAsync()
        {
            try
            {
                if (user != null)
                {
                    foreach (var cookie in NSHttpCookieStorage.SharedStorage.Cookies)
                        NSHttpCookieStorage.SharedStorage.DeleteCookie(cookie);

                    await TodoItemManager.DefaultManager.CurrentClient.LogoutAsync();

                    var logoutAlert = new UIAlertView("Authentication", $"You are now logged out - {user.UserId}", null, "OK", null);
                    logoutAlert.Show();
                }

                user = null;

                return true;
            }
            catch (Exception ex)
            {
                var logoutAlert = new UIAlertView("Logout failed", ex.Message, null, "OK", null);
                logoutAlert.Show();

                return false;
            }
        }
    }
}

