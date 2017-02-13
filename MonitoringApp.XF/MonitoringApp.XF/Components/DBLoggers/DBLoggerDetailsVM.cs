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
        public List<string> UnsubscribedPairs { get; set; } = DBLoggerManager.Instance.GetUnsubscribedPairs();

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
        }

        public async Task Refresh()
        {
            string dbLoggerName = DBLogger.DBLoggerName;

            await DBLoggerManager.Instance.LoadDBLoggerSubscriptionStatuses(true);

            UnsubscribedPairs = DBLoggerManager.Instance.GetUnsubscribedPairs();

            GetDBLoggerByName(dbLoggerName);
        }

        public async Task<GenericActionResult> SubscribePair(string dbLoggerName, string pair)
        {
            return await DBLoggerManager.Instance.SubscribePair(dbLoggerName, CrossUtils.GetFromStr(pair));
        }

        public async Task<GenericActionResult> UnsubscribePair(string dbLoggerName, string pair)
        {
            return await DBLoggerManager.Instance.UnsubscribePair(dbLoggerName, CrossUtils.GetFromStr(pair));
        }
    }
}
