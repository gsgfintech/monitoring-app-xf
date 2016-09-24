using Microsoft.WindowsAzure.MobileServices;
using MonitoringApp.XF.Components.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MonitoringApp.XF
{
    public class App : Application
    {
        public static IAuthenticate Authenticator { get; private set; }

        public static MobileServiceClient MobileServiceClient { get; private set; }

        public App()
        {
            MobileServiceClient = new MobileServiceClient(Constants.ApplicationURL);

            // The root page of your application
            //MainPage = new LoginPage();
            MainPage = new MainPage();
        }

        public static void Init(IAuthenticate authenticator)
        {
            Authenticator = authenticator;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
