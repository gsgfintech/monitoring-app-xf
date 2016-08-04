using System;

using Xamarin.Forms;

namespace Capital.GSG.FX.Monitor.App.XF
{
    public partial class LoginPage : ContentPage
    {
        bool authenticated = false;

        public LoginPage()
        {
            InitializeComponent();
        }

        async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            try
            {
                if (App.Authenticator != null)
                    authenticated = await App.Authenticator.AuthenticateAsync();

                if (authenticated)
                {
                    //Navigation.InsertPageBefore(new TodoList(), this);
                    Navigation.InsertPageBefore(new ExecutionsList(), this);
                    await Navigation.PopAsync();
                }
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message.Contains("Authentication was cancelled"))
                    messageLabel.Text = "Authentication cancelled by the user";
            }
            catch (Exception)
            {
                messageLabel.Text = "Authentication failed";
            }
        }
    }
}
