using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace MonitoringApp.XF.Components.Login
{
    public class LoginPage : ContentPage
    {
        private ContentPage sender;

        private bool authenticated = false;

        private Label messageLabel;

        public LoginPage(ContentPage sender)
        {
            this.sender = sender;

            messageLabel = new Label() { Text = "Sign in" };

            Content = new StackLayout
            {
                Children = {
                    messageLabel
                }
            };
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                if (App.Authenticator != null)
                    authenticated = await App.Authenticator.AuthenticateAsync();

                if (authenticated)
                {
                    if (sender != null)
                        await Navigation.PopAsync();
                    //await Navigation.PushAsync(sender);
                    else
                    {
                        if (Device.OS == TargetPlatform.iOS)
                            await Navigation.PopToRootAsync();

                        Application.Current.MainPage = new MainPage();
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message.Contains("Authentication was cancelled"))
                    messageLabel.Text = "Authentication cancelled by the user";
                else
                    messageLabel.Text = $"Authentication failed: {ex.Message}";
            }
            catch (Exception ex)
            {
                messageLabel.Text = $"Authentication failed: {ex.Message}";
            }
        }
    }
}
