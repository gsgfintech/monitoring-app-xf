using Capital.GSG.FX.Data.Core.ContractData;
using Capital.GSG.FX.Data.Core.SystemData;
using Capital.GSG.FX.Utils.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringApp.XF.Components.DBLoggers
{
    public class DBLoggerVM : BaseViewModel
    {
        private string dbLoggerName;
        public string DBLoggerName
        {
            get { return dbLoggerName; }
            set
            {
                if (dbLoggerName != value)
                {
                    dbLoggerName = value;
                    OnPropertyChanged(nameof(DBLoggerName));
                }
            }
        }

        public ObservableCollection<Cross> SubscribedPairs { get; set; }

        public string SubscribedPairsStr
        {
            get
            {
                var subscribedPairs = SubscribedPairs?.OrderBy(c => c);

                if (!subscribedPairs.IsNullOrEmpty())
                    return string.Join(", ", subscribedPairs);
                else
                    return string.Empty;
            }
        }
    }

    public static class DBLoggerVMExtensions
    {
        public static DBLoggerVM ToDBLoggerVM(this DBLoggerSubscriptionStatus dbLogger)
        {
            if (dbLogger == null)
                return null;

            return new DBLoggerVM()
            {
                DBLoggerName = dbLogger.DBLoggerName,
                SubscribedPairs = dbLogger.SubscribedPairs.ToObservableCollection()
            };
        }

        private static ObservableCollection<Cross> ToObservableCollection(this IEnumerable<Cross> pairs)
        {
            ObservableCollection<Cross> col = new ObservableCollection<Cross>();

            if (!pairs.IsNullOrEmpty())
            {
                foreach (var pair in pairs)
                    col.Add(pair);
            }

            return col;
        }
    }
}
