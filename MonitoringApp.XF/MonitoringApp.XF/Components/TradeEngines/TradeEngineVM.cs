using Capital.GSG.FX.Data.Core.ContractData;
using Capital.GSG.FX.Data.Core.Strategy;
using Capital.GSG.FX.Data.Core.SystemData;
using Capital.GSG.FX.Utils.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MonitoringApp.XF.Components.TradeEngines
{
    public class TradeEngineVM : BaseViewModel
    {
        private string engineName;
        public string EngineName
        {
            get { return engineName; }
            set
            {
                if (engineName != value)
                {
                    engineName = value;
                    OnPropertyChanged(nameof(EngineName));
                }
            }
        }

        public ObservableCollection<TradeEngineConfigStrategyVM> Strats { get; set; }

        public ObservableCollection<TradeEngineTradingStatusCrossVM> Crosses { get; set; }

        public IEnumerable<string> AllCrosses => Crosses?.Select(s => s.Cross.ToString()).OrderBy(c => c);

        public IEnumerable<string> TradingCrosses => Crosses?.Where(c => c.IsTrading)?.Select(s => s.Cross.ToString()).OrderBy(c => c);

        public string TradingCrossesStr
        {
            get
            {
                if (!TradingCrosses.IsNullOrEmpty())
                    return string.Join(", ", TradingCrosses);
                else
                    return string.Empty;
            }
        }

        public IEnumerable<string> NonTradingCrosses => Crosses?.Where(c => !c.IsTrading)?.Select(s => s.Cross.ToString()).OrderBy(c => c);

        public string NonTradingCrossesStr
        {
            get
            {
                if (!NonTradingCrosses.IsNullOrEmpty())
                    return string.Join(", ", NonTradingCrosses);
                else
                    return string.Empty;
            }
        }

        public string ActiveStrategiesStr
        {
            get
            {
                var activeStrategies = Strats?.Where(s => s.Active)?.Select(s => $"{s.Name}-{s.Version}").OrderBy(s => s);

                if (!activeStrategies.IsNullOrEmpty())
                    return string.Join(", ", activeStrategies);
                else
                    return string.Empty;
            }
        }

        public string InactiveStrategiesStr
        {
            get
            {
                var activeStrategies = Strats?.Where(s => !s.Active)?.Select(s => $"{s.Name}-{s.Version}").OrderBy(s => s);

                if (!activeStrategies.IsNullOrEmpty())
                    return string.Join(", ", activeStrategies);
                else
                    return string.Empty;
            }
        }

        public string TradingStrategiesStr
        {
            get
            {
                var tradingStrategies = Strats?.Where(s => s.Trading)?.Select(s => $"{s.Name}-{s.Version}").OrderBy(s => s);

                if (!tradingStrategies.IsNullOrEmpty())
                    return string.Join(", ", tradingStrategies);
                else
                    return string.Empty;
            }
        }

        public string NonTradingStrategiesStr
        {
            get
            {
                var nonTradingStrategies = Strats?.Where(s => !s.Trading)?.Select(s => $"{s.Name}-{s.Version}").OrderBy(s => s);

                if (!nonTradingStrategies.IsNullOrEmpty())
                    return string.Join(", ", nonTradingStrategies);
                else
                    return string.Empty;
            }
        }
    }

    public class TradeEngineConfigStrategyVM : BaseViewModel
    {
        private bool active;
        public bool Active
        {
            get { return active; }
            set
            {
                if (active != value)
                {
                    active = value;
                    OnPropertyChanged(nameof(Active));
                }
            }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        private bool trading;
        public bool Trading
        {
            get { return trading; }
            set
            {
                if (trading != value)
                {
                    trading = value;
                    OnPropertyChanged(nameof(Trading));
                }
            }
        }

        private string version;
        public string Version
        {
            get { return version; }
            set
            {
                if (version != value)
                {
                    version = value;
                    OnPropertyChanged(nameof(Version));
                }
            }
        }
    }

    public class TradeEngineTradingStatusCrossVM : BaseViewModel
    {
        private Cross cross;
        public Cross Cross
        {
            get { return cross; }
            set
            {
                if (cross != value)
                {
                    cross = value;
                    OnPropertyChanged(nameof(Cross));
                }
            }
        }

        private bool isTrading;
        public bool IsTrading
        {
            get { return isTrading; }
            set
            {
                if (isTrading != value)
                {
                    isTrading = value;
                    OnPropertyChanged(nameof(IsTrading));
                }
            }
        }
    }

    public static class TradeEngineVMExtensions
    {
        private static TradeEngineTradingStatusCrossVM ToTradeEngineTradingStatusCrossVM(this TradeEngineTradingStatusCross teCrossStatus)
        {
            if (teCrossStatus == null)
                return null;

            return new TradeEngineTradingStatusCrossVM()
            {
                Cross = teCrossStatus.Cross,
                IsTrading = teCrossStatus.IsTrading
            };
        }

        public static ObservableCollection<TradeEngineTradingStatusCrossVM> ToTradeEngineTradingStatusCrossVMs(this IEnumerable<TradeEngineTradingStatusCross> teCrossStatuses)
        {
            if (teCrossStatuses.IsNullOrEmpty())
                return new ObservableCollection<TradeEngineTradingStatusCrossVM>();

            ObservableCollection<TradeEngineTradingStatusCrossVM> col = new ObservableCollection<TradeEngineTradingStatusCrossVM>();

            foreach (var teCrossStatus in teCrossStatuses)
                col.Add(teCrossStatus.ToTradeEngineTradingStatusCrossVM());

            return col;
        }

        public static TradeEngineVM ToTradeEngineVM(this TradeEngineTradingStatus te)
        {
            if (te == null)
                return null;

            return new TradeEngineVM()
            {
                Crosses = te.Crosses.ToTradeEngineTradingStatusCrossVMs(),
                EngineName = te.EngineName,
                Strats = te.Strats.ToTradeEngineConfigStrategyVMs(),
            };
        }
    }

    public static class TradeEngineConfigStrategyVMExtensions
    {
        private static TradeEngineConfigStrategyVM ToTradeEngineConfigStrategyVM(this TradeEngineConfigStrategy strat)
        {
            if (strat == null)
                return null;

            return new TradeEngineConfigStrategyVM()
            {
                Active = strat.Active,
                Name = strat.Name,
                Trading = strat.Trading,
                Version = strat.Version
            };
        }

        public static ObservableCollection<TradeEngineConfigStrategyVM> ToTradeEngineConfigStrategyVMs(this IEnumerable<TradeEngineConfigStrategy> strats)
        {
            if (strats.IsNullOrEmpty())
                return new ObservableCollection<TradeEngineConfigStrategyVM>();

            ObservableCollection<TradeEngineConfigStrategyVM> col = new ObservableCollection<TradeEngineConfigStrategyVM>();

            foreach (var strat in strats)
                col.Add(strat.ToTradeEngineConfigStrategyVM());

            return col;
        }
    }
}
