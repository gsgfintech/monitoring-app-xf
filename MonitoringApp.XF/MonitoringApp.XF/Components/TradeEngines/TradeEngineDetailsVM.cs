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

        public void GetTradeEngineByName(string engineName)
        {
            TradeEngine = TradeEngineManager.Instance.GetTradeEngineTradingStatus(engineName).ToTradeEngineVM();
        }
    }
}
