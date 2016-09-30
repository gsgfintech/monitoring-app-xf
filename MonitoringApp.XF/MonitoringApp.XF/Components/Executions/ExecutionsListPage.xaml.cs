﻿using Xamarin.Forms;

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
            ExecutionSlimViewModel execution = ((ListView)sender).SelectedItem as ExecutionSlimViewModel;

            if (execution != null)
            {
                var detailsView = new ExecutionDetailsPage();
                await detailsView.ViewModel?.GetExecutionById(execution.Id);
                await Navigation.PushAsync(detailsView);
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await vm?.RefreshTodaysExecutions(false);
        }
    }
}