
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Content;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Capital.GSG.FX.Utils.Core.Logging;
using Microsoft.Extensions.Logging;
using Xamarin.Forms;
using Plugin.Toasts;

namespace MonitoringApp.XF.Droid
{
    [Activity(Label = "Stratedge.me Monitor", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            GSGLoggerFactory.Instance.AddDebug();

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            DependencyService.Register<ToastNotification>();
            ToastNotification.Init(this);

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

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            AuthenticationAgentContinuationHelper.SetAuthenticationAgentContinuationEventArgs(requestCode, resultCode, data);
        }
    }
}

