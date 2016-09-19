using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace MonitoringApp.XF.UWP
{
    public sealed partial class MainPage : IAuthenticate
    {
        private MobileServiceUser user;

        public MainPage()
        {
            this.InitializeComponent();

            XF.App.Init(this);

            LoadApplication(new XF.App());
        }

        public async Task<bool> AuthenticateAsync()
        {
            try
            {
                if (user == null)
                    user = await XF.App.MobileServiceClient?.LoginAsync(MobileServiceAuthenticationProvider.WindowsAzureActiveDirectory);

                return true;
            }
            catch (Exception ex)
            {
                await new MessageDialog(ex.Message, "Authentication failed").ShowAsync();
                return false;
            }
        }
    }
}
