using Capital.GSG.FX.Data.Core.SystemData;
using Capital.GSG.FX.Monitoring.Server.Connector;
using Capital.GSG.FX.Utils.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MonitoringApp.XF.Components.TradeEngines
{
    public class TradeEngineManager
    {
        private readonly BackendTradeEngineConnector tradeEngineConnector;

        private Dictionary<string, TradeEngineTradingStatus> tradeEngineTradingStatuses;

        private static TradeEngineManager instance;
        public static TradeEngineManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new TradeEngineManager();

                return instance;
            }
        }

        private TradeEngineManager()
        {
            tradeEngineConnector = App.MonitoringServerConnector.TradeEngineConnector;
        }

        public async Task<List<TradeEngineTradingStatus>> LoadTradeEngineTradingStatuses(bool refresh)
        {
            try
            {
                if (tradeEngineTradingStatuses == null || refresh)
                {
                    CancellationTokenSource cts = new CancellationTokenSource();
                    cts.CancelAfter(TimeSpan.FromMinutes(1));

                    var list = await tradeEngineConnector.RequestTradeEnginesTradingStatus(cts.Token);

                    if (!list.IsNullOrEmpty())
                        tradeEngineTradingStatuses = list.ToDictionary(te => te.EngineName, te => te);
                    else
                        tradeEngineTradingStatuses = null;
                }

                return tradeEngineTradingStatuses?.Values.ToList();
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Not retrieving trade engines trading statuses: operation cancelled");
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to retrieve trade engines trading statuses: {ex.Message}");
                return null;
            }
        }

        public TradeEngineTradingStatus GetTradeEngineTradingStatus(string engineName)
        {
            try
            {
                if (string.IsNullOrEmpty(engineName))
                    throw new ArgumentNullException(nameof(engineName));

                if (tradeEngineTradingStatuses != null && tradeEngineTradingStatuses.ContainsKey(engineName) && tradeEngineTradingStatuses[engineName] != null)
                    return tradeEngineTradingStatuses[engineName];
                else
                    return null;
            }
            catch (ArgumentNullException ex)
            {
                Debug.WriteLine($"Not retrieving trade engine trading status: missing or invalid parameter {ex.ParamName}");
                return null;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Failed to retrieve trade engine trading status: {e.Message}");
                return null;
            }
        }
    }
}
