using Xamarin.Forms;

namespace MonitoringApp.XF.Components.Positions
{
    public partial class PositionDetailsPage : ContentPage
    {
        public PositionDetailsVM ViewModel { get { return vm; } }

        public PositionDetailsPage()
        {
            InitializeComponent();
        }
    }
}
