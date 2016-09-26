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

            //openAlertsDataGrid.PullToRefreshCommand = new Command(ExecutePullToRefreshCommand);
        }

        private async void ExecutePullToRefreshCommand()
        {
            await RefreshAlerts(true);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await RefreshAlerts(false);
        }

        private async Task RefreshAlerts(bool refresh)
        {
            await vm?.RefreshOpenAlerts(refresh);
        }

        //private async void OnSelected(object sender, GridSelectionChangedEventArgs e)
        //{
        //    var item = openAlertsDataGrid.SelectedItem as AlertSlim;

        //    if (item != null)
        //    {
        //        var alert = await vm.GetAlertById(item.Id);
        //        var detailsView = new AlertDetailsPage(alert);
        //        await Navigation.PushAsync(detailsView);
        //    }

        //    openAlertsDataGrid.SelectedItem = null;
        //}
    }
}
