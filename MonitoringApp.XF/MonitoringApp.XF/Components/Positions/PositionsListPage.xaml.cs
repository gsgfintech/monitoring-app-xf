using Capital.GSG.FX.Data.Core.AccountPortfolio;
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

            await vm?.RefreshPositions(false);
        }

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Position position = ((ListView)sender).SelectedItem as Position;

            if (position != null)
            {
                var detailsView = new PositionDetailsPage();
                await detailsView.ViewModel?.GetPositionByCross(position.Cross);
                await Navigation.PushAsync(detailsView);
            }

            ((ListView)sender).SelectedItem = null;
        }
    }
}
