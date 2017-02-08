using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;
using Capital.GSG.FX.Utils.Core;

namespace MonitoringApp.XF.Components.Positions
{
    public class PositionsListVM : BaseViewModel
    {
        public ObservableCollection<PositionViewModel> Positions { get; set; } = new ObservableCollection<PositionViewModel>();
        public ObservableCollection<AccountViewModel> Accounts { get; set; } = new ObservableCollection<AccountViewModel>();

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

        private double totalPnlUsd;
        public double TotalPnlUsd
        {
            get { return totalPnlUsd; }
            set
            {
                if (totalPnlUsd != value)
                {
                    totalPnlUsd = value;
                    OnPropertyChanged(nameof(TotalPnlUsd));
                }
            }
        }

        public PositionsListVM()
        {
            RefreshCommand = new Command(ExecuteRefreshCommand, () => !IsRefreshing);
        }

        private async void ExecuteRefreshCommand()
        {
            await Refresh(true);

            IsRefreshing = false;
        }

        public async Task Refresh(bool refresh)
        {
            await LoadPositions(refresh);
            await LoadAccounts(refresh);
        }

        private async Task LoadPositions(bool refresh)
        {
            var positions = await PositionManager.Instance.LoadPositions(refresh);

            Positions.Clear();

            if (!positions.IsNullOrEmpty())
            {
                foreach (var position in positions)
                    Positions.Add(position.ToPositionViewModel());

                TotalPnlUsd = Positions.Select(p => p.TotalPnlUsd).Sum();
            }
        }

        private async Task LoadAccounts(bool refresh)
        {
            var accounts = await PositionManager.Instance.LoadAccounts(refresh);

            Accounts.Clear();

            if (!accounts.IsNullOrEmpty())
            {
                foreach (var account in accounts)
                    Accounts.Add(account.ToAccountViewModel());
            }
        }
    }
}
