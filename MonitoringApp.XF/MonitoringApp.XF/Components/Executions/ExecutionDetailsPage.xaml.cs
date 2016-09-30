using Xamarin.Forms;

namespace MonitoringApp.XF.Components.Executions
{
    public partial class ExecutionDetailsPage : ContentPage
    {
        public ExecutionDetailsVM ViewModel { get { return vm; } }

        public ExecutionDetailsPage()
        {
            InitializeComponent();
        }
    }
}
