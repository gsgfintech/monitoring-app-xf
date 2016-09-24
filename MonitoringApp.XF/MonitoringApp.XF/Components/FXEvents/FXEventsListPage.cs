using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MonitoringApp.XF.Components.FXEvents
{
    public class FXEventsListPage : ContentPage
    {
        private FXEventsListVM vm;

        private ListView todaysHighImpactLstView;
        private ListView currentWeeksLstView;

        public FXEventsListPage()
        {
            vm = new FXEventsListVM();

            todaysHighImpactLstView = new ListView();
            todaysHighImpactLstView.ItemTemplate = new DataTemplate(typeof(TextCell));
            todaysHighImpactLstView.ItemTemplate.SetBinding(TextCell.TextProperty, "Item2");
            todaysHighImpactLstView.ItemTemplate.SetBinding(TextCell.DetailProperty, "Item3");
            todaysHighImpactLstView.IsPullToRefreshEnabled = true;
            todaysHighImpactLstView.ItemSelected += OnSelected;
            todaysHighImpactLstView.Refreshing += OnRefresh;

            currentWeeksLstView = new ListView();
            currentWeeksLstView.ItemTemplate = new DataTemplate(typeof(TextCell));
            currentWeeksLstView.ItemTemplate.SetBinding(TextCell.TextProperty, "Item2");
            currentWeeksLstView.ItemTemplate.SetBinding(TextCell.DetailProperty, "Item3");
            currentWeeksLstView.IsPullToRefreshEnabled = true;
            currentWeeksLstView.ItemSelected += OnSelected;
            currentWeeksLstView.Refreshing += OnRefresh;

            Content = new StackLayout
            {
                Children =
                {
                    new Label() { Text = "Today's High Impact", FontAttributes = FontAttributes.Bold },
                    todaysHighImpactLstView,
                    new Label() { Text = "Current Week", FontAttributes = FontAttributes.Bold },
                    currentWeeksLstView
                }
            };
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await RefreshFXEventsLists(false);
        }

        private async void OnRefresh(object sender, EventArgs e)
        {
            var list = (ListView)sender;

            try
            {
                await RefreshFXEventsLists(true);
            }
            catch (Exception) { }
            finally
            {
                list.EndRefresh();
            }
        }

        private async void OnSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as Tuple<string, string, string>;

            if (item != null)
            {
                var fxEvent = await vm.GetFXEventById(item.Item1);
                var detailsView = new FXEventDetailsPage(fxEvent);
                await Navigation.PushAsync(detailsView);
            }

            currentWeeksLstView.SelectedItem = null;
            todaysHighImpactLstView.SelectedItem = null;
        }

        private async Task RefreshFXEventsLists(bool refresh)
        {
            currentWeeksLstView.ItemsSource = await vm.RefreshCurrentWeeksFXEvents(refresh);
            todaysHighImpactLstView.ItemsSource = await vm.RefreshTodaysHighImpactFXEvents(refresh);
        }
    }
}
