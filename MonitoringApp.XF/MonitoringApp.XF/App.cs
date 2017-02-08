using Capital.GSG.FX.Monitoring.Server.Connector;
using Xamarin.Forms;

namespace MonitoringApp.XF
{
    public class App : Application
    {
        public static MonitoringServerConnector MonitoringServerConnector { get; private set; }

        public App()
        {
            AccessTokenProvider accessTokenProvider = new AccessTokenProvider();
            MonitoringServerConnector = new MonitoringServerConnector(Constants.MonitoringBackendAddress, accessTokenProvider);

            // The root page of your application
            //MainPage = new LoginPage();
            MainPage = new MainPage();
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
