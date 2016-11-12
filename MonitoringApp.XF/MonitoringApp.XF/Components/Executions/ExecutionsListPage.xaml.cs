using MonitoringApp.XF.ViewModels;
using Xamarin.Forms;

namespace MonitoringApp.XF.Components.Executions
{
    public partial class ExecutionsListPage : ContentPage
    {
        public ExecutionsListPage()
        {
            InitializeComponent();
        }

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ExecutionViewModel execution = ((ListView)sender).SelectedItem as ExecutionViewModel;

            if (execution != null)
            {
                var detailsView = new ExecutionDetailsPage();
                await detailsView.ViewModel?.GetExecutionById(execution.Id);
                await Navigation.PushAsync(detailsView);
            }

            ((ListView)sender).SelectedItem = null;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await vm?.RefreshExecutions(false);
        }
    }
}
