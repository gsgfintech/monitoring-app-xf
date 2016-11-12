using MonitoringApp.XF.Components.Alerts;
using MonitoringApp.XF.Components.Executions;
using MonitoringApp.XF.Components.Orders;
using MonitoringApp.XF.Components.PnL;
using MonitoringApp.XF.Components.SystemsStatus;
using System;
using Xamarin.Forms;

namespace MonitoringApp.XF.Components.Home
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await vm?.RefreshAll(false);
        }

        private async void GotoPnL(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PnLPage());
        }

        private async void GotoTrades(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ExecutionsListPage());
        }

        private async void GotoOrders(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new OrdersListPage());
        }

        private async void GotoAlerts(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AlertsListPage());
        }

        private async void GotoSystems(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SystemsStatusesListPage());
        }
    }
}
