using Capital.GSG.FX.Utils.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MonitoringApp.XF.Components.TradeEngines
{
    public partial class TradeEngineDetailsPage : ContentPage
    {
        public TradeEngineDetailsVM ViewModel { get { return vm; } }

        private List<string> allCrosses = new List<string>();
        private List<string> tradingCrosses = new List<string>();
        private List<string> nonTradingCrosses = new List<string>();

        private List<Tuple<string, string>> tradingStrats = new List<Tuple<string, string>>();
        private List<Tuple<string, string>> nonTradingStrats = new List<Tuple<string, string>>();

        private List<Tuple<string, string>> activeStrats = new List<Tuple<string, string>>();
        private List<Tuple<string, string>> inactiveStrats = new List<Tuple<string, string>>();

        public TradeEngineDetailsPage()
        {
            InitializeComponent();

            startTradingCrossesPicker.SelectedIndexChanged += OnStartTradingCrossesPickerSelectedIndexChanged;
            stopTradingCrossesPicker.SelectedIndexChanged += OnStopTradingCrossesPickerSelectedIndexChanged;

            activateStratsPicker.SelectedIndexChanged += OnActivateStratsPickerSelectedIndexChanged;
            deactivateStratsPicker.SelectedIndexChanged += OnDeactivateStratsPickerSelectedIndexChanged;

            startTradingStratsPicker.SelectedIndexChanged += OnStartTradingStratsPickerSelectedIndexChanged;
            stopTradingStratsPicker.SelectedIndexChanged += OnStopTradingStratsPickerSelectedIndexChanged;

            closePositionsPicker.SelectedIndexChanged += OnClosePositionsPickerSelectedIndexChanged;
            cancelOrdersPicker.SelectedIndexChanged += OnCancelOrdersPickerSelectedIndexChanged;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            PopulateAll();
        }

        private void PopulateAll()
        {
            PopulateStartTradingCrossesPicker();
            PopulateStopTradingCrossesPicker();

            PopulateActivateStratsPicker();
            PopulateDeactivateStratsPicker();

            PopulateStartTradingStratsPicker();
            PopulateStopTradingStratsPicker();

            PopulateClosePositionsAndCancelOrdersPickers();
        }

        private async void OnStopTradingStratsPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ViewModel.IsBusy)
                return;

            ViewModel.IsBusy = true;

            if (stopTradingStratsPicker.SelectedIndex > -1)
            {
                string engineName = ViewModel.TradeEngine.EngineName;
                string stratName = tradingStrats[stopTradingStratsPicker.SelectedIndex].Item1;
                string stratVersion = tradingStrats[stopTradingStratsPicker.SelectedIndex].Item2;

                if (await DisplayAlert("STOP TRADING", $"Stop trading {stratName}-{stratVersion} on {engineName}?", "YES", "NO"))
                {
                    var result = await ViewModel.StopTradingStrategy(engineName, stratName, stratVersion);

                    if (result?.Success == true)
                    {
                        await DisplayAlert($"STOP TRADING {stratName}={stratVersion}", $"Successfully stopped trading of {stratName}-{stratVersion} on {engineName}", "OK");

                        await Refresh();
                    }
                    else
                        await DisplayAlert($"STOP TRADING {stratName}-{stratVersion}", $"Failed: {result.Message}", "OK");
                }
            }

            stopTradingStratsPicker.SelectedIndex = -1;

            ViewModel.IsBusy = false;
        }

        private async void OnStartTradingStratsPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ViewModel.IsBusy)
                return;

            ViewModel.IsBusy = true;

            if (startTradingStratsPicker.SelectedIndex > -1)
            {
                string engineName = ViewModel.TradeEngine.EngineName;
                string stratName = nonTradingStrats[startTradingStratsPicker.SelectedIndex].Item1;
                string stratVersion = nonTradingStrats[startTradingStratsPicker.SelectedIndex].Item2;

                if (await DisplayAlert("START TRADING", $"Start trading {stratName}-{stratVersion} on {engineName}?", "YES", "NO"))
                {
                    var result = await ViewModel.StartTradingStrategy(engineName, stratName, stratVersion);

                    if (result?.Success == true)
                    {
                        await DisplayAlert($"START TRADING {stratName}={stratVersion}", $"Successfully started trading of {stratName}-{stratVersion} on {engineName}", "OK");

                        await Refresh();
                    }
                    else
                        await DisplayAlert($"START TRADING {stratName}-{stratVersion}", $"Failed: {result.Message}", "OK");
                }
            }

            stopTradingStratsPicker.SelectedIndex = -1;

            ViewModel.IsBusy = false;
        }

        private async void OnDeactivateStratsPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ViewModel.IsBusy)
                return;

            ViewModel.IsBusy = true;

            if (deactivateStratsPicker.SelectedIndex > -1)
            {
                string engineName = ViewModel.TradeEngine.EngineName;
                string stratName = activeStrats[deactivateStratsPicker.SelectedIndex].Item1;
                string stratVersion = activeStrats[deactivateStratsPicker.SelectedIndex].Item2;

                if (await DisplayAlert("DEACTIVATE", $"Deactivate {stratName}-{stratVersion} on {engineName}?", "YES", "NO"))
                {
                    var result = await ViewModel.DeactivateStrategy(engineName, stratName, stratVersion);

                    if (result?.Success == true)
                    {
                        await DisplayAlert($"DEACTIVATE {stratName}={stratVersion}", $"Successfully deactivated {stratName}-{stratVersion} on {engineName}", "OK");

                        await Refresh();
                    }
                    else
                        await DisplayAlert($"DEACTIVATE {stratName}-{stratVersion}", $"Failed: {result.Message}", "OK");
                }
            }

            deactivateStratsPicker.SelectedIndex = -1;

            ViewModel.IsBusy = false;
        }

        private async void OnActivateStratsPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ViewModel.IsBusy)
                return;

            ViewModel.IsBusy = true;

            if (activateStratsPicker.SelectedIndex > -1)
            {
                string engineName = ViewModel.TradeEngine.EngineName;
                string stratName = inactiveStrats[activateStratsPicker.SelectedIndex].Item1;
                string stratVersion = inactiveStrats[activateStratsPicker.SelectedIndex].Item2;

                if (await DisplayAlert("ACTIVATE", $"Activate {stratName}-{stratVersion} on {engineName}?", "YES", "NO"))
                {
                    var result = await ViewModel.ActivateStrategy(engineName, stratName, stratVersion);

                    if (result?.Success == true)
                    {
                        await DisplayAlert($"ACTIVATE {stratName}={stratVersion}", $"Successfully activated {stratName}-{stratVersion} on {engineName}", "OK");

                        await Refresh();
                    }
                    else
                        await DisplayAlert($"ACTIVATE {stratName}-{stratVersion}", $"Failed: {result.Message}", "OK");
                }
            }

            activateStratsPicker.SelectedIndex = -1;

            ViewModel.IsBusy = false;
        }

        private async Task Refresh()
        {
            await ViewModel.Refresh();

            PopulateAll();
        }

        private void PopulateStopTradingCrossesPicker()
        {
            stopTradingStratsPicker.Items.Clear();


            if (!ViewModel.TradeEngine.TradingCrosses.IsNullOrEmpty())
            {
                tradingCrosses = new List<string>((new string[1] { "ALL" }).Concat(ViewModel.TradeEngine.TradingCrosses));

                foreach (var item in tradingCrosses)
                    stopTradingCrossesPicker.Items.Add(item);

                stopTradingCrossesPicker.IsEnabled = true;
            }
            else
                stopTradingCrossesPicker.IsEnabled = false;
        }

        private async void OnStopTradingCrossesPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ViewModel.IsBusy)
                return;

            ViewModel.IsBusy = true;

            if (stopTradingCrossesPicker.SelectedIndex > -1)
            {
                string engineName = ViewModel.TradeEngine.EngineName;
                string cross = tradingCrosses[stopTradingCrossesPicker.SelectedIndex];

                if (await DisplayAlert("STOP TRADING", $"Stop trading {cross} on {engineName}?", "YES", "NO"))
                {
                    var result = await ViewModel.StopTradingCross(engineName, cross);

                    if (result?.Success == true)
                    {
                        await DisplayAlert($"STOP TRADING {cross}", $"Successfully stopped trading of {cross} on {engineName}", "OK");

                        await Refresh();
                    }
                    else
                        await DisplayAlert($"STOP TRADING {cross}", $"Failed: {result.Message}", "OK");
                }
            }

            stopTradingCrossesPicker.SelectedIndex = -1;

            ViewModel.IsBusy = false;
        }

        private async void OnCancelOrdersPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ViewModel.IsBusy)
                return;

            ViewModel.IsBusy = true;

            if (cancelOrdersPicker.SelectedIndex > -1)
            {
                string engineName = ViewModel.TradeEngine.EngineName;
                string cross = allCrosses[cancelOrdersPicker.SelectedIndex];

                if (await DisplayAlert("CANCEL ORDERS", $"Cancel orders for {cross} on {engineName}?", "YES", "NO"))
                {
                    var result = await ViewModel.CancelOrders(engineName, cross);

                    if (result?.Success == true)
                    {
                        await DisplayAlert($"CANCEL ORDERS FOR {cross}", $"Successfully cancelled orders for {cross} on {engineName}", "OK");

                        await Refresh();
                    }
                    else
                        await DisplayAlert($"CANCEL ORDERS FOR {cross}", $"Failed: {result.Message}", "OK");
                }
            }

            cancelOrdersPicker.SelectedIndex = -1;

            ViewModel.IsBusy = false;
        }

        private async void OnClosePositionsPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ViewModel.IsBusy)
                return;

            ViewModel.IsBusy = true;

            if (closePositionsPicker.SelectedIndex > -1)
            {
                string engineName = ViewModel.TradeEngine.EngineName;
                string cross = allCrosses[closePositionsPicker.SelectedIndex];

                if (await DisplayAlert("CLOSE POSITION", $"Close {cross} positions on {engineName}?", "YES", "NO"))
                {
                    var result = await ViewModel.ClosePosition(engineName, cross);

                    if (result?.Success == true)
                    {
                        await DisplayAlert($"CLOSE {cross} POSITIONS", $"Successfully closed {cross} positions on {engineName}", "OK");

                        await Refresh();
                    }
                    else
                        await DisplayAlert($"CLOSE {cross} POSITIONS", $"Failed: {result.Message}", "OK");
                }
            }

            closePositionsPicker.SelectedIndex = -1;

            ViewModel.IsBusy = false;
        }

        private void PopulateStartTradingCrossesPicker()
        {
            startTradingCrossesPicker.Items.Clear();


            if (!ViewModel.TradeEngine.NonTradingCrosses.IsNullOrEmpty())
            {
                nonTradingCrosses = new List<string>((new string[1] { "ALL" }).Concat(ViewModel.TradeEngine.NonTradingCrosses));

                foreach (var item in nonTradingCrosses)
                    startTradingCrossesPicker.Items.Add(item);

                startTradingCrossesPicker.IsEnabled = true;
            }
            else
                startTradingCrossesPicker.IsEnabled = false;
        }

        private void PopulateClosePositionsAndCancelOrdersPickers()
        {
            closePositionsPicker.Items.Clear();
            cancelOrdersPicker.Items.Clear();


            if (!ViewModel.TradeEngine.AllCrosses.IsNullOrEmpty())
            {
                allCrosses = new List<string>((new string[1] { "ALL" }).Concat(ViewModel.TradeEngine.AllCrosses));

                foreach (var item in allCrosses)
                {
                    closePositionsPicker.Items.Add(item);
                    cancelOrdersPicker.Items.Add(item);
                }

                closePositionsPicker.IsEnabled = true;
                cancelOrdersPicker.IsEnabled = true;
            }
            else
            {
                closePositionsPicker.IsEnabled = false;
                cancelOrdersPicker.IsEnabled = false;
            }
        }

        private async void OnStartTradingCrossesPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ViewModel.IsBusy)
                return;

            ViewModel.IsBusy = true;

            if (startTradingCrossesPicker.SelectedIndex > -1)
            {
                string engineName = ViewModel.TradeEngine.EngineName;
                string cross = nonTradingCrosses[startTradingCrossesPicker.SelectedIndex];

                if (await DisplayAlert("START TRADING", $"Start trading {cross} on {engineName}?", "YES", "NO"))
                {
                    var result = await ViewModel.StartTradingCross(engineName, cross);

                    if (result?.Success == true)
                    {
                        await DisplayAlert($"START TRADING {cross}", $"Successfully started trading of {cross} on {engineName}", "OK");

                        await Refresh();
                    }
                    else
                        await DisplayAlert($"START TRADING {cross}", $"Failed: {result.Message}", "OK");
                }
            }

            startTradingCrossesPicker.SelectedIndex = -1;

            ViewModel.IsBusy = false;
        }

        private void PopulateDeactivateStratsPicker()
        {
            deactivateStratsPicker.Items.Clear();

            activeStrats = ViewModel?.TradeEngine.Strats?.Where(s => s.Active)?.Select(s => new Tuple<string, string>(s.Name, s.Version)).OrderBy(s => s, StringTupleComparer.Instance).ToList();

            if (!activeStrats.IsNullOrEmpty())
            {
                foreach (var item in activeStrats)
                    deactivateStratsPicker.Items.Add($"{item.Item1}-{item.Item2}");

                deactivateStratsPicker.IsEnabled = true;
            }
            else
                deactivateStratsPicker.IsEnabled = false;
        }

        private void PopulateActivateStratsPicker()
        {
            activateStratsPicker.Items.Clear();

            inactiveStrats = ViewModel?.TradeEngine.Strats?.Where(s => !s.Active)?.Select(s => new Tuple<string, string>(s.Name, s.Version)).OrderBy(s => s, StringTupleComparer.Instance).ToList();

            if (!inactiveStrats.IsNullOrEmpty())
            {
                foreach (var item in inactiveStrats)
                    activateStratsPicker.Items.Add($"{item.Item1}-{item.Item2}");

                activateStratsPicker.IsEnabled = true;
            }
            else
                activateStratsPicker.IsEnabled = false;
        }

        private void PopulateStopTradingStratsPicker()
        {
            stopTradingStratsPicker.Items.Clear();

            tradingStrats = ViewModel?.TradeEngine.Strats?.Where(s => s.Trading)?.Select(s => new Tuple<string, string>(s.Name, s.Version)).OrderBy(s => s, StringTupleComparer.Instance).ToList();

            if (!tradingStrats.IsNullOrEmpty())
            {
                foreach (var item in tradingStrats)
                    stopTradingStratsPicker.Items.Add($"{item.Item1}-{item.Item2}");

                stopTradingStratsPicker.IsEnabled = true;
            }
            else
                stopTradingStratsPicker.IsEnabled = false;
        }

        private void PopulateStartTradingStratsPicker()
        {
            startTradingStratsPicker.Items.Clear();

            nonTradingStrats = ViewModel?.TradeEngine.Strats?.Where(s => !s.Trading)?.Select(s => new Tuple<string, string>(s.Name, s.Version)).OrderBy(s => s, StringTupleComparer.Instance).ToList();

            if (!nonTradingStrats.IsNullOrEmpty())
            {
                foreach (var item in nonTradingStrats)
                    startTradingStratsPicker.Items.Add($"{item.Item1}-{item.Item2}");

                startTradingStratsPicker.IsEnabled = true;
            }
            else
                startTradingStratsPicker.IsEnabled = false;
        }

        private class StringTupleComparer : IComparer<Tuple<string, string>>
        {
            private static StringTupleComparer instance;
            public static StringTupleComparer Instance
            {
                get
                {
                    if (instance == null)
                        instance = new StringTupleComparer();

                    return instance;
                }
            }

            private StringTupleComparer() { }

            public int Compare(Tuple<string, string> x, Tuple<string, string> y)
            {
                if (x.Item1 == y.Item1)
                    return x.Item2.CompareTo(y.Item2);
                else
                    return x.Item1.CompareTo(y.Item1);
            }
        }
    }
}
