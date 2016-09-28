using Xamarin.Forms;

namespace MonitoringApp.XF.Components.Orders
{
    public partial class OrderDetailsPage : ContentPage
    {
        public OrderDetailsVM ViewModel { get { return vm; } }

        public OrderDetailsPage()
        {
            InitializeComponent();
        }

        private void OnHistoryPointSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }
    }
}
