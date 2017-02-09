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

        public string TradingCrossesStr
        {
            get
            {
                var tradingCrosses = Strats?.Where(s => s.Trading)?.Select(s => s.Cross.ToString()).OrderBy(c => c);

                if (!tradingCrosses.IsNullOrEmpty())
                    return string.Join(", ", tradingCrosses);
                else
                    return string.Empty;
            }
        }

        public string NonTradingCrossesStr
        {
            get
            {
                var nonTradingCrosses = Strats?.Where(s => !s.Trading)?.Select(s => s.Cross.ToString()).OrderBy(c => c);

                if (!nonTradingCrosses.IsNullOrEmpty())
                    return string.Join(", ", nonTradingCrosses);
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

    public static class TradeEngineVMExtensions
    {
        public static TradeEngineVM ToTradeEngineVM(this TradeEngineTradingStatus te)
        {
            if (te == null)
                return null;

            return new TradeEngineVM()
            {
                EngineName = te.EngineName,
                Strats = te.Strats.ToTradeEngineConfigStrategyVMs(),
            };
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

    public static class TradeEngineConfigStrategyVMExtensions
    {
        private static TradeEngineConfigStrategyVM ToTradeEngineConfigStrategyVM(this TradeEngineConfigStrategy strat)
        {
            if (strat == null)
                return null;

            return new TradeEngineConfigStrategyVM()
            {
                Active = strat.Active,
                Cross = strat.Cross,
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
