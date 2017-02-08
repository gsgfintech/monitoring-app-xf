using MonitoringApp.XF.Managers;
using MonitoringApp.XF.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MonitoringApp.XF.Components.PnL
{
    public class PnLPageVM : BaseViewModel
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

        public PnLPageVM()
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

            await RefreshPnL(false);
        }

        private async void ExecuteRefreshCommand()
        {
            await RefreshPnL(true);

            IsRefreshing = false;
        }

        public async Task RefreshPnL(bool refresh)
        {
            await LoadPnL(refresh);
        }

        private async Task LoadPnL(bool refresh)
        {
            PnL = (await PnLManager.Instance.LoadPnL(Day, refresh)).ToPnLViewModel();
        }
    }
}
