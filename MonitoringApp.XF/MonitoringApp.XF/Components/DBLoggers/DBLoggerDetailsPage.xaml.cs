using Capital.GSG.FX.Utils.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace MonitoringApp.XF.Components.DBLoggers
{
    public partial class DBLoggerDetailsPage : ContentPage
    {
        public DBLoggerDetailsVM ViewModel { get { return vm; } }

        private List<string> subscribedCrosses = new List<string>();
        private List<string> unsubscribedCrosses = new List<string>();

        public DBLoggerDetailsPage()
        {
            InitializeComponent();

            unsubscribeCrossesPicker.SelectedIndexChanged += OnUnsubscribeCrossesPickerSelectedIndexChanged;
            subscribeCrossesPicker.SelectedIndexChanged += OnSubscribeCrossesPickerSelectedIndexChanged;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            PopulateAll();
        }

        private async Task Refresh()
        {
            await ViewModel.Refresh();

            PopulateAll();
        }

        private void PopulateAll()
        {
            PopulateUnsubscribeCrossesPicker();
            PopulateSubscribeCrossesPicker();
        }

        private void PopulateUnsubscribeCrossesPicker()
        {
            unsubscribeCrossesPicker.Items.Clear();

            subscribedCrosses = ViewModel?.DBLogger.SubscribedPairs?.Select(c => c.ToString()).OrderBy(c => c).ToList();

            if (!subscribedCrosses.IsNullOrEmpty())
            {
                foreach (var item in subscribedCrosses)
                    unsubscribeCrossesPicker.Items.Add(item);

                unsubscribeCrossesPicker.IsEnabled = true;
            }
            else
                unsubscribeCrossesPicker.IsEnabled = false;
        }

        private void PopulateSubscribeCrossesPicker()
        {
            subscribeCrossesPicker.Items.Clear();

            unsubscribedCrosses = ViewModel.UnsubscribedPairs;

            if (!unsubscribedCrosses.IsNullOrEmpty())
            {
                foreach (var item in unsubscribedCrosses)
                    subscribeCrossesPicker.Items.Add(item);

                subscribeCrossesPicker.IsEnabled = true;
            }
            else
                subscribeCrossesPicker.IsEnabled = false;
        }

        private async void OnUnsubscribeCrossesPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ViewModel.IsBusy)
                return;

            ViewModel.IsBusy = true;

            if (unsubscribeCrossesPicker.SelectedIndex > -1)
            {
                string dbLoggerName = ViewModel.DBLogger.DBLoggerName;
                string cross = subscribedCrosses[unsubscribeCrossesPicker.SelectedIndex];

                if (await DisplayAlert("UNSUBSCRIBE", $"Unsubscribe {cross} on {dbLoggerName}?", "YES", "NO"))
                {
                    var result = await ViewModel.UnsubscribePair(dbLoggerName, cross);

                    if (result.Success)
                    {
                        await Refresh();
                        await Utils.ShowToastNotification("UNSUBSCRIBE: SUCCESS", $"Unsubscribed {cross} on {dbLoggerName}");
                    }
                    else
                        await Utils.ShowToastNotification("UNSUBSCRIBE: FAILED", result.Message);
                }
            }

            unsubscribeCrossesPicker.SelectedIndex = -1;

            ViewModel.IsBusy = false;
        }

        private async void OnSubscribeCrossesPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ViewModel.IsBusy)
                return;

            ViewModel.IsBusy = true;

            if (subscribeCrossesPicker.SelectedIndex > -1)
            {
                string dbLoggerName = ViewModel.DBLogger.DBLoggerName;
                string cross = unsubscribedCrosses[subscribeCrossesPicker.SelectedIndex];

                if (await DisplayAlert("SUBSCRIBE", $"Subscribe {cross} on {dbLoggerName}?", "YES", "NO"))
                {
                    var result = await ViewModel.SubscribePair(dbLoggerName, cross);

                    if (result.Success)
                    {
                        await Refresh();
                        await Utils.ShowToastNotification("SUBSCRIBE: SUCCESS", $"Subscribed {cross} on {dbLoggerName}");
                    }
                    else
                        await Utils.ShowToastNotification("SUBSCRIBE: FAILED", result.Message);
                }
            }

            subscribeCrossesPicker.SelectedIndex = -1;

            ViewModel.IsBusy = false;
        }
    }
}
