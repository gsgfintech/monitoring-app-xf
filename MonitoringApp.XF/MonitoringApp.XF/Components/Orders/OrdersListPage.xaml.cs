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

            await ViewModel?.RefreshOrders(false);
        }

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            OrderSlimViewModel order = ((ListView)sender).SelectedItem as OrderSlimViewModel;

            if (order != null)
            {
                var detailsView = new OrderDetailsPage();
                await detailsView.ViewModel?.GetOrderByPermanentId(order.PermanentId);
                await Navigation.PushAsync(detailsView);
            }
        }
    }
}
