using Capital.GSG.FX.Monitoring.AppDataTypes;
using System;
using Xamarin.Forms;

namespace MonitoringApp.XF.Components.SystemsStatus
{
    public partial class SystemsStatusesListPage : ContentPage
    {
        private bool isStartingOrStopping;

        public SystemsStatusesListPage()
        {
            InitializeComponent();
        }

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = ((ListView)sender).SelectedItem as SystemStatusSlim;

            if (item != null)
            {
                var detailsView = new SystemStatusDetailsPage();
                await detailsView.ViewModel?.GetSystemStatusByName(item.Name);
                await Navigation.PushAsync(detailsView);
            }

            ((ListView)sender).SelectedItem = null;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await vm?.RefreshSystemsStatuses(false);
        }

        private async void OnStartStopButtonClicked(object sender, EventArgs e)
        {
            string label = ((Button)sender).Text;
            string name = ((Button)sender).CommandParameter?.ToString();

            if (string.IsNullOrEmpty(name))
                return;

            if (isStartingOrStopping)
                return;

            isStartingOrStopping = true;

            if (await DisplayAlert(label, $"{label} system {name}?", "Go", "Cancel"))
            {
                GenericActionResult result = await vm?.DoStartStop(name);

                if (result != null)
                {
                    string prefix;

                    if (result.Success)
                        prefix = "Success";
                    else
                        prefix = "Failed";

                    await DisplayAlert($"System {label}", $"{prefix}: {result.Message}", "OK");
                }
            }

            isStartingOrStopping = false;
        }
    }
}
