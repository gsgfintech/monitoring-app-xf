using Capital.GSG.FX.Monitoring.AppDataTypes;
using Syncfusion.SfDataGrid.XForms;
using Xamarin.Forms;

namespace MonitoringApp.XF.Components.Orders
{
    public partial class OrdersListPage : ContentPage
    {
        public OrdersListPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await ViewModel?.RefreshTodaysOrders(false);
        }

        private async void OnItemSelected(object sender, GridSelectionChangedEventArgs e)
        {
            OrderSlim order = ((SfDataGrid)sender).SelectedItem as OrderSlim;

            if (order != null)
            {
                var detailsView = new OrderDetailsPage();
                await detailsView.ViewModel?.GetOrderByPermanentId(order.PermanentId);
                await Navigation.PushAsync(detailsView);
            }
        }
    }
}
