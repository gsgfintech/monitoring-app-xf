using Capital.GSG.FX.Monitoring.AppDataTypes;
using System;
using Xamarin.Forms;

namespace MonitoringApp.XF.Components.Alerts
{
    public partial class AlertsListPage : ContentPage
    {
        public AlertsListPage()
        {
            InitializeComponent();
        }

        private async void CloseAllButtonClicked(object sender, EventArgs e)
        {
            await vm?.CloseAllAlertsAuthenticated();
        }

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            AlertSlim alert = ((ListView)sender).SelectedItem as AlertSlim;

            if (alert != null)
            {
                var detailsView = new AlertDetailsPage();
                await detailsView.ViewModel?.GetAlertById(alert.Id);
                await Navigation.PushAsync(detailsView);
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await vm?.RefreshAlerts(false);
        }
    }
}
