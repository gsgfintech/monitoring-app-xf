using Xamarin.Forms;

namespace MonitoringApp.XF.Components.Positions
{
    public partial class PositionsListPage : ContentPage
    {
        private PositionsListVM vm;

        public PositionsListPage()
        {
            InitializeComponent();

            vm = BindingContext as PositionsListVM;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await vm?.Refresh(false);
        }

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            PositionViewModel position = ((ListView)sender).SelectedItem as PositionViewModel;

            if (position != null)
            {
                var detailsView = new PositionDetailsPage();
                await detailsView.ViewModel?.GetPositionByCross(position.Broker, position.Account, position.Cross);
                await Navigation.PushAsync(detailsView);
            }

            ((ListView)sender).SelectedItem = null;
        }

        private async void OnAccountItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            AccountViewModel account = ((ListView)sender).SelectedItem as AccountViewModel;

            if (account != null)
            {
                var detailsView = new AccountDetailsPage();
                await detailsView.ViewModel?.GetAccount(account.Broker, account.Name);
                await Navigation.PushAsync(detailsView);
            }

            ((ListView)sender).SelectedItem = null;
        }
    }
}
