
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;

namespace Capital.GSG.FX.Monitor.App.XF
{
    public class App : Application
	{
        public static IAuthenticate Authenticator { get; private set; }
        public static MobileServiceClient Client { get; private set; }

        public App()
        {
            Client = new MobileServiceClient(Constants.ApplicationURL);

            // The root page of your application
            MainPage = new NavigationPage(new LoginPage());
        }

        public static void Init(IAuthenticate authenticator)
        {
            Authenticator = authenticator;
        }

        protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

