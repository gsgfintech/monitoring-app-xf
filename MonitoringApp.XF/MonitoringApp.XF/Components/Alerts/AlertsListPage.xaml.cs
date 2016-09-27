using Capital.GSG.FX.Monitoring.AppDataTypes;
using Syncfusion.SfDataGrid.XForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MonitoringApp.XF.Components.Alerts
{
    public partial class AlertsListPage : ContentPage
    {
        private AlertsListVM vm;

        public AlertsListPage()
        {
            InitializeComponent();

            vm = BindingContext as AlertsListVM;
        }

        private async void OnShowDetails(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);

            var item = mi.CommandParameter as AlertSlim;

            if (item != null)
            {
                var alert = await vm.GetAlertById(item.Id);
                var detailsView = new AlertDetailsPage(alert);
                await Navigation.PushAsync(detailsView);
            }
        }

        public async void OnClose(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);

            var item = mi.CommandParameter as AlertSlim;

            if (item != null)
            {
                if (await DisplayAlert("Close Alert", $"Close alert '{item.Subject}'?", "OK", "Cancel"))
                {
                    // TODO : close
                }
            }
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await vm?.RefreshOpenAlerts(false);
        }
    }
}
