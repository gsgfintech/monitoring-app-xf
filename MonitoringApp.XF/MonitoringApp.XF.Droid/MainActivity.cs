using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

namespace MonitoringApp.XF.Droid
{
    [Activity(Label = "MonitoringApp.XF", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IAuthenticate
    {
        private MobileServiceUser user;

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            CurrentPlatform.Init();

            App.Init(this);

            LoadApplication(new App());
        }

        public async Task<bool> AuthenticateAsync()
        {
            try
            {
                if (user == null)
                    user = await App.MobileServiceClient?.LoginAsync(this, MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory);

                return true;
            }
            catch (Exception ex)
            {
                CreateAndShowDialog(ex.Message, "Authentication failed");
                return false;
            }
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

