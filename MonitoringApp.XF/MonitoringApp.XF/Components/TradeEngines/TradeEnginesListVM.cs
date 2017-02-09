using Capital.GSG.FX.Utils.Core;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MonitoringApp.XF.Components.TradeEngines
{
    public class TradeEnginesListVM : BaseViewModel
    {
        public ObservableCollection<TradeEngineVM> TradeEngines { get; set; } = new ObservableCollection<TradeEngineVM>();

        public Command RefreshCommand { get; private set; }

        private bool isRefreshing;
        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set
            {
                if (value != isRefreshing)
                {
                    isRefreshing = value;
                    OnPropertyChanged(nameof(IsRefreshing));
                }
            }
        }

        public TradeEnginesListVM()
        {
            RefreshCommand = new Command(ExecuteRefreshCommand, () => !IsRefreshing);
        }

        private async void ExecuteRefreshCommand()
        {
            await RefreshTradeEngines(true);

            IsRefreshing = false;
        }

        internal async Task RefreshTradeEngines(bool refresh)
        {
            var tradeEngines = (await TradeEngineManager.Instance.LoadTradeEngineTradingStatuses(refresh))?.AsEnumerable().OrderBy(te => te.EngineName);

            TradeEngines.Clear();

            if (!tradeEngines.IsNullOrEmpty())
            {
                foreach (var tradeEngine in tradeEngines)
                    TradeEngines.Add(tradeEngine.ToTradeEngineVM());
            }
        }
    }
}
