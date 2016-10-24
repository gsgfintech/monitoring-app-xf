using Capital.GSG.FX.Utils.Portable;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;

namespace MonitoringApp.XF.Components.Positions
{
    public class PositionsListVM : BaseViewModel
    {
        public ObservableCollection<PositionViewModel> Positions { get; set; } = new ObservableCollection<PositionViewModel>();

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
            await RefreshPositions(true);

            IsRefreshing = false;
        }

        public async Task RefreshPositions(bool refresh)
        {
            try
            {
                await LoadPositions(refresh);
            }
            catch (AuthenticationRequiredException)
            {
                if (App.Authenticator != null)
                {
                    bool authenticated = await App.Authenticator.AuthenticateAsync();

                    if (authenticated)
                        await LoadPositions(refresh);
                }
            }
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
    }
}
