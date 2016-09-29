using Xamarin.Forms;

namespace MonitoringApp.XF.Components.Alerts
{
    public partial class AlertDetailsPage : ContentPage
    {
        public AlertDetailsVM ViewModel { get { return vm; } }

        public AlertDetailsPage()
        {
            InitializeComponent();
        }
    }
}
