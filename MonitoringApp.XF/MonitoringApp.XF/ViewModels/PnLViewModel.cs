using Capital.GSG.FX.Data.Core.AccountPortfolio;
using Capital.GSG.FX.Data.Core.ContractData;
using Capital.GSG.FX.Utils.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MonitoringApp.XF.ViewModels
{
    public class PnLViewModel : BaseViewModel
    {
        private DateTime lastUpdate;
        public DateTime LastUpdate
        {
            get { return lastUpdate; }
            set
            {
                if (lastUpdate != value)
                {
                    lastUpdate = value;
                    OnPropertyChanged(nameof(lastUpdate));
                }
            }
        }

        public ObservableCollection<PnLPerCrossViewModel> PerCrossPnLs { get; set; }

        private double totalFees;
        public double TotalFees
        {
            get { return totalFees; }
            set
            {
                if (totalFees != value)
                {
                    totalFees = value;
                    OnPropertyChanged(nameof(TotalFees));
                }
            }
        }

        private double totalGrossRealized;
        public double TotalGrossRealized
        {
            get { return totalGrossRealized; }
            set
            {
                if (totalGrossRealized != value)
                {
                    totalGrossRealized = value;
                    OnPropertyChanged(nameof(TotalGrossRealized));
                }
            }
        }

        private double totalGrossUnrealized;
        public double TotalGrossUnrealized
        {
            get { return totalGrossUnrealized; }
            set
            {
                if (totalGrossUnrealized != value)
                {
                    totalGrossUnrealized = value;
                    OnPropertyChanged(nameof(TotalGrossUnrealized));
                }
            }
        }

        private double totalNetRealized;
        public double TotalNetRealized
        {
            get { return totalNetRealized; }
            set
            {
                if (totalNetRealized != value)
                {
                    totalNetRealized = value;
                    OnPropertyChanged(nameof(TotalNetRealized));
                }
            }
        }

        private double totalTradesCount;
        public double TotalTradesCount
        {
            get { return totalTradesCount; }
            set
            {
                if (totalTradesCount != value)
                {
                    totalTradesCount = value;
                    OnPropertyChanged(nameof(TotalTradesCount));
                }
            }
        }

        public PnLViewModel()
        {
            PerCrossPnLs = new ObservableCollection<PnLPerCrossViewModel>();
        }

        public PnLViewModel(DateTime lastUpdate, ObservableCollection<PnLPerCrossViewModel> perCrossPnls, double totalFees, double totalGrossRealized, double totalGrossUnrealized, double totalNetRealized, double totalTradesCount)
        {
            this.lastUpdate = lastUpdate;
            PerCrossPnLs = perCrossPnls;
            this.totalFees = totalFees;
            this.totalGrossRealized = totalGrossRealized;
            this.totalGrossUnrealized = totalGrossUnrealized;
            this.totalNetRealized = totalNetRealized;
            this.totalTradesCount = totalTradesCount;
        }
    }

    public class PnLPerCrossViewModel : BaseViewModel
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

        private double grossRealized;
        public double GrossRealized
        {
            get { return grossRealized; }
            set
            {
                if (grossRealized != value)
                {
                    grossRealized = value;
                    OnPropertyChanged(nameof(GrossRealized));
                }
            }
        }

        private double grossUnrealized;
        public double GrossUnrealized
        {
            get { return grossUnrealized; }
            set
            {
                if (grossUnrealized != value)
                {
                    grossUnrealized = value;
                    OnPropertyChanged(nameof(GrossUnrealized));
                }
            }
        }

        private double netRealized;
        public double NetRealized
        {
            get { return netRealized; }
            set
            {
                if (netRealized != value)
                {
                    netRealized = value;
                    OnPropertyChanged(nameof(NetRealized));
                }
            }
        }

        private double totalFees;
        public double TotalFees
        {
            get { return totalFees; }
            set
            {
                if (totalFees != value)
                {
                    totalFees = value;
                    OnPropertyChanged(nameof(TotalFees));
                }
            }
        }

        private double tradesCount;
        public double TradesCount
        {
            get { return tradesCount; }
            set
            {
                if (tradesCount != value)
                {
                    tradesCount = value;
                    OnPropertyChanged(nameof(TradesCount));
                }
            }
        }

        public PnLPerCrossViewModel() { }

        public PnLPerCrossViewModel(Cross cross, double grossRealized, double grossUnrealized, double netRealized, double totalFees, double tradesCount)
        {
            this.cross = cross;
            this.grossRealized = grossRealized;
            this.grossUnrealized = grossUnrealized;
            this.netRealized = netRealized;
            this.totalFees = totalFees;
            this.tradesCount = tradesCount;
        }
    }

    public static class PnLViewModelExtensions
    {
        private static PnLPerCrossViewModel ToPnLPerCrossViewModel(this PnLPerCross pnl)
        {
            if (pnl == null)
                return null;

            return new PnLPerCrossViewModel(pnl.Cross, pnl.GrossRealized, pnl.GrossUnrealized, pnl.NetRealized, pnl.TotalFees, pnl.TradesCount);
        }

        private static ObservableCollection<PnLPerCrossViewModel> ToPnLPerCrossViewModel(this IEnumerable<PnLPerCross> pnls)
        {
            ObservableCollection<PnLPerCrossViewModel> retVal = new ObservableCollection<ViewModels.PnLPerCrossViewModel>();

            if (!pnls.IsNullOrEmpty())
            {
                foreach (var pnl in pnls)
                    retVal.Add(pnl.ToPnLPerCrossViewModel());
            }

            return retVal;
        }

        public static PnLViewModel ToPnLViewModel(this PnL pnl)
        {
            if (pnl == null)
                return null;

            return new PnLViewModel(pnl.LastUpdate.LocalDateTime, pnl.PerCrossPnLs.ToPnLPerCrossViewModel(), pnl.TotalFees, pnl.TotalGrossRealized, pnl.TotalGrossUnrealized, pnl.TotalNetRealized, pnl.TotalTradesCount);
        }
    }
}
