using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MonitoringApp.XF.Components.SystemsStatus
{
    public class SystemsStatusesListPage : ContentPage
    {
        private SystemsStatusesListVM vm;

        private ListView systemsStatusesLstView;

        public SystemsStatusesListPage()
        {
            vm = new SystemsStatusesListVM();

            systemsStatusesLstView = new ListView();
            systemsStatusesLstView.ItemTemplate = new DataTemplate(typeof(TextCell));
            systemsStatusesLstView.ItemTemplate.SetBinding(TextCell.TextProperty, "Item1");
            systemsStatusesLstView.ItemTemplate.SetBinding(TextCell.DetailProperty, "Item2");
            systemsStatusesLstView.ItemTemplate.SetBinding(BackgroundColorProperty, "Item3");
            systemsStatusesLstView.IsPullToRefreshEnabled = true;
            systemsStatusesLstView.ItemSelected += OnSelected;
            systemsStatusesLstView.Refreshing += OnRefresh;

            Title = "Systems";

            Content = new StackLayout
            {
                Children =
                {
                    systemsStatusesLstView
                }
            };
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await RefreshList(false);
        }

        private async void OnRefresh(object sender, EventArgs e)
        {
            var list = (ListView)sender;

            try
            {
                await RefreshList(true);
            }
            catch (Exception) { }
            finally
            {
                list.EndRefresh();
            }
        }

        private async Task RefreshList(bool refresh)
        {
            systemsStatusesLstView.ItemsSource = await vm.RefreshSystemsStatuses(refresh);
        }

        private async void OnSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as Tuple<string, string, Color>;

            if (item != null)
            {
                var system = await vm.GetSystemStatusByName(item.Item1);
                var detailsView = new SystemStatusDetailsPage(system);
                await Navigation.PushAsync(detailsView);
            }

            systemsStatusesLstView.SelectedItem = null;
        }
    }
}
