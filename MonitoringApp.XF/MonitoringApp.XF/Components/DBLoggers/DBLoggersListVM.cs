using Capital.GSG.FX.Data.Core.ContractData;
using Capital.GSG.FX.Utils.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MonitoringApp.XF.Components.DBLoggers
{
    public class DBLoggersListVM : BaseViewModel
    {
        public ObservableCollection<DBLoggerVM> DBLoggers { get; set; } = new ObservableCollection<DBLoggerVM>();

        private string unsubscribedPairsStr;
        public string UnsubscribedPairsStr
        {
            get { return unsubscribedPairsStr; }
            set
            {
                if (unsubscribedPairsStr != value)
                {
                    unsubscribedPairsStr = value;
                    OnPropertyChanged(nameof(UnsubscribedPairsStr));
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

        public DBLoggersListVM()
        {
            RefreshCommand = new Command(ExecuteRefreshCommand, () => !IsRefreshing);
        }

        private async void ExecuteRefreshCommand()
        {
            await RefreshDBLoggers(true);

            IsRefreshing = false;
        }

        internal async Task RefreshDBLoggers(bool refresh)
        {
            var dbLoggers = (await DBLoggerManager.Instance.LoadDBLoggerSubscriptionStatuses(refresh))?.AsEnumerable().OrderBy(dbLogger => dbLogger.DBLoggerName);

            DBLoggers.Clear();

            if (!dbLoggers.IsNullOrEmpty())
            {
                foreach (var dbLogger in dbLoggers)
                    DBLoggers.Add(dbLogger.ToDBLoggerVM());
            }

            UnsubscribedPairsStr = string.Join(", ", DBLoggerManager.Instance.GetUnsubscribedPairs());
        }
    }
}
