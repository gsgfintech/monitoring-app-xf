using Capital.GSG.FX.Data.Core.WebApi;
using System.Threading.Tasks;

namespace MonitoringApp.XF.Components.TradeEngines
{
    public class TradeEngineDetailsVM : BaseViewModel
    {
        private TradeEngineVM tradeEngine;
        public TradeEngineVM TradeEngine
        {
            get { return tradeEngine; }
            set
            {
                if (tradeEngine != value)
                {
                    tradeEngine = value;
                    OnPropertyChanged(nameof(TradeEngine));
                }
            }
        }

        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                if (isBusy != value)
                {
                    isBusy = value;
                    OnPropertyChanged(nameof(IsBusy));
                }
            }
        }

        public void GetTradeEngineByName(string engineName)
        {
            TradeEngine = TradeEngineManager.Instance.GetTradeEngineTradingStatus(engineName).ToTradeEngineVM();
        }

        public async Task Refresh()
        {
            string engineName = TradeEngine.EngineName;

            await TradeEngineManager.Instance.LoadTradeEngineTradingStatuses(true);

            GetTradeEngineByName(engineName);
        }

        public async Task<GenericActionResult> StopTradingCross(string engineName, string cross)
        {
            return await TradeEngineManager.Instance.StopTradingCross(engineName, cross);
        }

        public async Task<GenericActionResult> StartTradingCross(string engineName, string cross)
        {
            return await TradeEngineManager.Instance.StartTradingCross(engineName, cross);
        }

        public async Task<GenericActionResult> StopTradingStrategy(string engineName, string stratName, string stratVersion)
        {
            return await TradeEngineManager.Instance.StopTradingStrategy(engineName, stratName, stratVersion);
        }

        public async Task<GenericActionResult> StartTradingStrategy(string engineName, string stratName, string stratVersion)
        {
            return await TradeEngineManager.Instance.StartTradingStrategy(engineName, stratName, stratVersion);
        }

        public async Task<GenericActionResult> DeactivateStrategy(string engineName, string stratName, string stratVersion)
        {
            return await TradeEngineManager.Instance.DeactivateStrategy(engineName, stratName, stratVersion);
        }

        public async Task<GenericActionResult> ActivateStrategy(string engineName, string stratName, string stratVersion)
        {
            return await TradeEngineManager.Instance.ActivateStrategy(engineName, stratName, stratVersion);
        }

        public async Task<GenericActionResult> ClosePosition(string engineName, string cross)
        {
            return await TradeEngineManager.Instance.ClosePosition(engineName, cross);
        }

        public async Task<GenericActionResult> CancelOrders(string engineName, string cross)
        {
            return await TradeEngineManager.Instance.CancelOrders(engineName, cross);
        }
    }
}
