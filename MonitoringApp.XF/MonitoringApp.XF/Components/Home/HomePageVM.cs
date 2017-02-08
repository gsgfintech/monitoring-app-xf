using Capital.GSG.FX.Data.Core.SystemData;
using Capital.GSG.FX.Utils.Core;
using MonitoringApp.XF.Managers;
using MonitoringApp.XF.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MonitoringApp.XF.Components.Home
{
    public class HomePageVM : BaseViewModel
    {
        private PnLViewModel pnl;
        public PnLViewModel PnL
        {
            get { return pnl; }
            set
            {
                pnl = value;
                OnPropertyChanged(nameof(PnL));
            }
        }

        private int systemsGreenCount;
        public int SystemsGreenCount
        {
            get { return systemsGreenCount; }
            set
            {
                if (systemsGreenCount != value)
                {
                    systemsGreenCount = value;
                    OnPropertyChanged(nameof(SystemsGreenCount));
                }
            }
        }

        private int systemsYellowCount;
        public int SystemsYellowCount
        {
            get { return systemsYellowCount; }
            set
            {
                if (systemsYellowCount != value)
                {
                    systemsYellowCount = value;
                    OnPropertyChanged(nameof(SystemsYellowCount));
                }
            }
        }

        private int systemsRedCount;
        public int SystemsRedCount
        {
            get { return systemsRedCount; }
            set
            {
                if (systemsRedCount != value)
                {
                    systemsRedCount = value;
                    OnPropertyChanged(nameof(SystemsRedCount));
                }
            }
        }

        private int alertsCount;
        public int AlertsCount
        {
            get { return alertsCount; }
            set
            {
                if (alertsCount != value)
                {
                    alertsCount = value;
                    OnPropertyChanged(nameof(AlertsCount));
                }
            }
        }

        private int tradesCount;
        public int TradesCount
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

        private int ordersCount;
        public int OrdersCount
        {
            get { return ordersCount; }
            set
            {
                if (ordersCount != value)
                {
                    ordersCount = value;
                    OnPropertyChanged(nameof(OrdersCount));
                }
            }
        }

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

        public HomePageVM()
        {
            RefreshCommand = new Command(ExecuteRefreshCommand, () => !IsRefreshing);
        }

        private async void ExecuteRefreshCommand()
        {
            await RefreshAll(true);

            IsRefreshing = false;
        }

        public async Task RefreshAll(bool refresh)
        {
            await RefreshPnL(refresh);
            await RefreshTradesCount(refresh);
            await RefreshOrdersCount(refresh);
            await RefreshAlertsCount(refresh);
            await RefreshSystemsStatusesCounts(refresh);
        }

        private async Task RefreshPnL(bool refresh)
        {
            await LoadPnL(refresh);
        }

        private async Task LoadPnL(bool refresh)
        {
            PnL = (await PnLManager.Instance.LoadPnL(DateTime.Today, refresh)).ToPnLViewModel();
        }

        private async Task RefreshTradesCount(bool refresh)
        {
            await LoadTradesCount(refresh);
        }

        private async Task LoadTradesCount(bool refresh)
        {
            TradesCount = (await ExecutionManager.Instance.LoadExecutions(DateTime.Today, refresh))?.Count ?? 0;
        }

        private async Task RefreshOrdersCount(bool refresh)
        {
            await LoadOrdersCount(refresh);
        }

        private async Task LoadOrdersCount(bool refresh)
        {
            OrdersCount = (await OrderManager.Instance.LoadOrders(DateTime.Today, refresh))?.Count ?? 0;
        }

        private async Task RefreshAlertsCount(bool refresh)
        {
            await LoadAlertsCount(refresh);
        }

        private async Task LoadAlertsCount(bool refresh)
        {
            AlertsCount = (await AlertManager.Instance.LoadOpenAlerts(refresh))?.Count ?? 0;
        }

        private async Task RefreshSystemsStatusesCounts(bool refresh)
        {
            await LoadSystemsStatusesCounts(refresh);
        }

        private async Task LoadSystemsStatusesCounts(bool refresh)
        {
            var systems = await SystemStatusManager.Instance.LoadSystemsStatuses(refresh);

            if (!systems.IsNullOrEmpty())
            {
                SystemsGreenCount = systems.Where(s => s.OverallStatus == SystemStatusLevel.GREEN).Count();
                SystemsYellowCount = systems.Where(s => s.OverallStatus == SystemStatusLevel.YELLOW).Count();
                SystemsRedCount = systems.Where(s => s.OverallStatus == SystemStatusLevel.RED).Count();
            }
        }
    }
}
