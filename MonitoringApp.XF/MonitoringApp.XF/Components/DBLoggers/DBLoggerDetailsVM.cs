using Capital.GSG.FX.Data.Core.ContractData;
using Capital.GSG.FX.Data.Core.WebApi;
using Capital.GSG.FX.Utils.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringApp.XF.Components.DBLoggers
{
    public class DBLoggerDetailsVM : BaseViewModel
    {
        private Dictionary<string, Cross> subscribedPairs = new Dictionary<string, Cross>();

        private DBLoggerVM dbLogger;
        public DBLoggerVM DBLogger
        {
            get { return dbLogger; }
            set
            {
                if (dbLogger != value)
                {
                    dbLogger = value;
                    OnPropertyChanged(nameof(DBLogger));
                }
            }
        }

        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                if (isBusy != value)
                {
                    isBusy = value;
                    OnPropertyChanged(nameof(IsBusy));
                }
            }
        }

        public void GetDBLoggerByName(string dbLoggerName)
        {
            DBLogger = DBLoggerManager.Instance.GetDBLoggerSubscriptionStatus(dbLoggerName).ToDBLoggerVM();

            if (!DBLogger.SubscribedPairs.IsNullOrEmpty())
                subscribedPairs = DBLogger.SubscribedPairs.ToDictionary(c => c.ToString(), c => c);
        }

        public async Task Refresh()
        {
            string dbLoggerName = DBLogger.DBLoggerName;

            await DBLoggerManager.Instance.LoadDBLoggerSubscriptionStatuses(true);

            GetDBLoggerByName(dbLoggerName);
        }

        public async Task<GenericActionResult> SubscribePair(string dbLoggerName, string pair)
        {
            return await DBLoggerManager.Instance.SubscribePair(dbLoggerName, subscribedPairs[pair]);
        }

        public async Task<GenericActionResult> UnsubscribePair(string dbLoggerName, string pair)
        {
            return await DBLoggerManager.Instance.UnsubscribePair(dbLoggerName, subscribedPairs[pair]);
        }
    }
}
