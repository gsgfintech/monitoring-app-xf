using Xamarin.Forms;

namespace MonitoringApp.XF.Components.TradeEngines
{
    public partial class TradeEnginesListPage : ContentPage
    {
        public TradeEnginesListPage()
        {
            InitializeComponent();
        }

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            TradeEngineVM tradeEngine = ((ListView)sender).SelectedItem as TradeEngineVM;

            if (tradeEngine != null)
            {
                var detailsView = new TradeEngineDetailsPage();
                detailsView.ViewModel?.GetTradeEngineByName(tradeEngine.EngineName);
                await Navigation.PushAsync(detailsView);
            }

            ((ListView)sender).SelectedItem = null;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await vm?.RefreshTradeEngines(false);
        }
    }
}
