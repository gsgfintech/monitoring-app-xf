using Capital.GSG.FX.Data.Core.ContractData;
using Capital.GSG.FX.Data.Core.SystemData;
using Capital.GSG.FX.Data.Core.WebApi;
using Capital.GSG.FX.Monitoring.Server.Connector;
using Capital.GSG.FX.Utils.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MonitoringApp.XF.Components.DBLoggers
{
    public class DBLoggerManager
    {
        private readonly BackendDBLoggerConnector dbLoggerConnector;

        private Dictionary<string, DBLoggerSubscriptionStatus> dbLoggersSubscriptionsStatuses;

        private static DBLoggerManager instance;
        public static DBLoggerManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new DBLoggerManager();

                return instance;
            }
        }

        private DBLoggerManager()
        {
            dbLoggerConnector = App.MonitoringServerConnector.DBLoggerConnector;
        }

        public async Task<List<DBLoggerSubscriptionStatus>> LoadDBLoggerSubscriptionStatuses(bool refresh)
        {
            try
            {
                if (dbLoggersSubscriptionsStatuses == null || refresh)
                {
                    CancellationTokenSource cts = new CancellationTokenSource();
                    cts.CancelAfter(TimeSpan.FromMinutes(1));

                    var list = await dbLoggerConnector.RequestDBLoggersSubscriptionsStatus(cts.Token);

                    if (!list.IsNullOrEmpty())
                        dbLoggersSubscriptionsStatuses = list.ToDictionary(te => te.DBLoggerName, te => te);
                    else
                        dbLoggersSubscriptionsStatuses = null;
                }

                return dbLoggersSubscriptionsStatuses?.Values.ToList();
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Not retrieving DB loggers subscriptions statuses: operation cancelled");
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to retrieve DB loggers subscriptions statuses: {ex.Message}");
                return null;
            }
        }

        public DBLoggerSubscriptionStatus GetDBLoggerSubscriptionStatus(string dbLoggerName)
        {
            try
            {
                if (string.IsNullOrEmpty(dbLoggerName))
                    throw new ArgumentNullException(nameof(dbLoggerName));

                if (dbLoggersSubscriptionsStatuses != null && dbLoggersSubscriptionsStatuses.ContainsKey(dbLoggerName) && dbLoggersSubscriptionsStatuses[dbLoggerName] != null)
                    return dbLoggersSubscriptionsStatuses[dbLoggerName];
                else
                    return null;
            }
            catch (ArgumentNullException ex)
            {
                Debug.WriteLine($"Not retrieving DB logger subscriptions status: missing or invalid parameter {ex.ParamName}");
                return null;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Failed to retrieve DB logger subscriptions status: {e.Message}");
                return null;
            }
        }

        public async Task<GenericActionResult> SubscribePair(string dbLoggerName, Cross pair)
        {
            try
            {
                CancellationTokenSource cts = new CancellationTokenSource();
                cts.CancelAfter(TimeSpan.FromMinutes(1));

                return await dbLoggerConnector.SubscribePairs(dbLoggerName, new Cross[1] { pair }, cts.Token);
            }
            catch (OperationCanceledException)
            {
                string err = "Not requesting to subscribe pairs: operation cancelled";
                Debug.WriteLine(err);
                return new GenericActionResult(false, err);
            }
            catch (Exception ex)
            {
                string err = $"Failed to request to subscribe pairs: {ex.Message}";
                Debug.WriteLine(err);
                return new GenericActionResult(false, err);
            }
        }

        public async Task<GenericActionResult> UnsubscribePair(string dbLoggerName, Cross pair)
        {
            try
            {
                CancellationTokenSource cts = new CancellationTokenSource();
                cts.CancelAfter(TimeSpan.FromMinutes(1));

                return await dbLoggerConnector.UnsubscribePairs(dbLoggerName, new Cross[1] { pair }, cts.Token);
            }
            catch (OperationCanceledException)
            {
                string err = "Not requesting to unsubscribe pairs: operation cancelled";
                Debug.WriteLine(err);
                return new GenericActionResult(false, err);
            }
            catch (Exception ex)
            {
                string err = $"Failed to request to unsubscribe pairs: {ex.Message}";
                Debug.WriteLine(err);
                return new GenericActionResult(false, err);
            }
        }
    }
}
