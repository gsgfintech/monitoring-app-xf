using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Android.Webkit;

namespace Capital.GSG.FX.Monitor.App.XF.Droid
{
    [Activity(Label = "Capital.GSG.FX.Monitor.App.XF.Droid",
        Icon = "@drawable/icon",
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
        Theme = "@android:style/Theme.Holo.Light")]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity, IAuthenticate
    {
        MobileServiceUser user;

        public async Task<bool> AuthenticateAsync()
        {
            try
            {
                if (user == null)
                {
                    //user = await TodoItemManager.DefaultManager.CurrentClient.LoginAsync(this, MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory);
                    user = await App.Client?.LoginAsync(this, MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory);

                    //if (user != null)
                    //    CreateAndShowDialog($"You are now logged in = {user.UserId}", "Logged In!");
                }

                return true;
            }
            catch (Exception ex)
            {
                CreateAndShowDialog(ex.Message, "Authentication failed");
                return false;
            }
        }

        public async Task<bool> LogoutAsync()
        {
            try
            {
                if (user != null)
                {
                    CookieManager.Instance.RemoveAllCookie();

                    //await TodoItemManager.DefaultManager.CurrentClient.LogoutAsync();
                    await App.Client?.LogoutAsync();

                    CreateAndShowDialog($"You are now logged out - {user.UserId}", "Logged out!");
                }

                user = null;

                return true;
            }
            catch (Exception ex)
            {
                CreateAndShowDialog(ex.Message, "Logout failed");
                return false;
            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();

            App.Init(this);

            LoadApplication(new App());
        }

        private void CreateAndShowDialog(string message, string title)
        {
            var builder = new AlertDialog.Builder(this);

            builder.SetMessage(message);
            builder.SetTitle(title);
            builder.SetNeutralButton("OK", (sender, args) => { });

            builder.Create().Show();
        }
    }
}

