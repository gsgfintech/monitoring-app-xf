using Capital.GSG.FX.Utils.Portable;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MonitoringApp.XF.Components.Executions
{
    public class ExecutionsListVM : BaseViewModel
    {
        public ObservableCollection<ExecutionViewModel> TodaysTrades { get; set; } = new ObservableCollection<ExecutionViewModel>();

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

        private DateTime newDay;
        public DateTime NewDay
        {
            get { return newDay; }
            set
            {
                if (newDay != value)
                {
                    newDay = value;
                    OnPropertyChanged(nameof(NewDay));
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

        public Command ChangeDayCommand { get; private set; }

        public ExecutionsListVM()
        {
            RefreshCommand = new Command(ExecuteRefreshCommand, () => !IsRefreshing);
            ChangeDayCommand = new Command(ExecuteChangeDayCommand);

            Day = DateTime.Today;
            NewDay = Day;
        }

        private async void ExecuteChangeDayCommand()
        {
            if (NewDay == Day)
                return;

            Day = NewDay;

            await RefreshExecutions(false);
        }

        private async void ExecuteRefreshCommand()
        {
            await RefreshExecutions(true);

            IsRefreshing = false;
        }

        public async Task RefreshExecutions(bool refresh)
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
            var trades = (await ExecutionManager.Instance.LoadExecutions(Day, refresh))?.AsEnumerable().OrderByDescending(t => t.ExecutionTime);

            TodaysTrades.Clear();

            if (!trades.IsNullOrEmpty())
            {
                foreach (var trade in trades)
                    TodaysTrades.Add(trade.ToExecutionViewModel());
            }
        }
    }
}
