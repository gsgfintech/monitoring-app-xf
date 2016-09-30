using Capital.GSG.FX.Utils.Portable;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MonitoringApp.XF.Components.Executions
{
    public class ExecutionsListVM : BaseViewModel
    {
        public ObservableCollection<ExecutionSlimViewModel> TodaysTrades { get; set; } = new ObservableCollection<ExecutionSlimViewModel>();

        private DateTime day;
        public DateTime Day
        {
            get { return day; }
            set
            {
                if (day != value)
                {
                    day = value;
                    OnPropertyChanged(nameof(Day));
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

        public ExecutionsListVM()
        {
            RefreshCommand = new Command(ExecuteRefreshCommand, () => !IsRefreshing);
            Day = DateTime.Today;
        }

        private async void ExecuteRefreshCommand()
        {
            await RefreshTodaysExecutions(true);

            IsRefreshing = false;
        }

        public async Task RefreshTodaysExecutions(bool refresh)
        {
            try
            {
                await LoadExecutions(refresh);
            }
            catch (AuthenticationRequiredException)
            {
                if (App.Authenticator != null)
                {
                    bool authenticated = await App.Authenticator.AuthenticateAsync();

                    if (authenticated)
                        await LoadExecutions(refresh);
                }
            }
        }

        private async Task LoadExecutions(bool refresh)
        {
            var trades = await ExecutionManager.Instance.LoadTodaysExecutions(refresh);

            TodaysTrades.Clear();

            if (!trades.IsNullOrEmpty())
            {
                foreach (var trade in trades)
                    TodaysTrades.Add(trade.ToExecutionSlimViewModel());
            }
        }
    }
}
