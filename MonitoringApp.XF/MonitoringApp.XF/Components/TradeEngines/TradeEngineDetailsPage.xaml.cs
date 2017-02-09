
using Xamarin.Forms;

namespace MonitoringApp.XF.Components.TradeEngines
{
    public partial class TradeEngineDetailsPage : ContentPage
    {
        public TradeEngineDetailsVM ViewModel { get { return vm; } }

        public TradeEngineDetailsPage()
        {
            InitializeComponent();
        }
    }
}
