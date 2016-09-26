using MonitoringApp.XF.Components.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MonitoringApp.XF.Components.Executions
{
    public class ExecutionsListPage : ContentPage
    {
        private ExecutionsListVM vm;
        private ListView lstView;

        public ExecutionsListPage()
        {
            vm = new ExecutionsListVM();
            lstView = new ListView();
            lstView.ItemTemplate = new DataTemplate(typeof(TextCell));
            lstView.ItemTemplate.SetBinding(TextCell.TextProperty, "Item2");
            lstView.ItemTemplate.SetBinding(TextCell.DetailProperty, "Item3");
            lstView.IsPullToRefreshEnabled = true;
            lstView.ItemSelected += OnSelected;
            lstView.Refreshing += OnRefresh;
            
            Content = lstView;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await RefreshTodaysExecutions(false);
        }

        private async void OnRefresh(object sender, EventArgs e)
        {
            var list = (ListView)sender;
            Exception error = null;

            try
            {
                await RefreshTodaysExecutions(true);
            }
            catch (Exception ex)
            {
                error = ex;
            }
            finally
            {
                list.EndRefresh();
            }

            if (error != null)
            {
                await DisplayAlert("Refresh Error", "Couldn't refresh data (" + error.Message + ")", "OK");
            }
        }

        private async void OnSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as Tuple<string, string, string>;

            if (item != null)
            {
                var execution = await vm.GetExecutionById(item.Item1);
                var detailsView = new ExecutionDetailsPage(execution);
                await Navigation.PushAsync(detailsView);
            }

            lstView.SelectedItem = null;
        }

        private async Task RefreshTodaysExecutions(bool refresh)
        {
            lstView.ItemsSource = await vm.RefreshTodaysExecutions(refresh);
        }
    }
}
