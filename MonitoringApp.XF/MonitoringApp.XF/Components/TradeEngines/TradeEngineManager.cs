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

        public async Task<GenericActionResult> StopTradingCross(string engineName, string cross)
        {
            try
            {
                CancellationTokenSource cts = new CancellationTokenSource();
                cts.CancelAfter(TimeSpan.FromMinutes(1));

                return await tradeEngineConnector.StopTrading(engineName, cross, cts.Token);
            }
            catch (OperationCanceledException)
            {
                string err = "Not requesting to stop trading cross: operation cancelled";
                Debug.WriteLine(err);
                return new GenericActionResult(false, err);
            }
            catch (Exception ex)
            {
                string err = $"Failed to request to stop trading cross: {ex.Message}";
                Debug.WriteLine(err);
                return new GenericActionResult(false, err);
            }
        }

        public async Task<GenericActionResult> StartTradingCross(string engineName, string cross)
        {
            try
            {
                CancellationTokenSource cts = new CancellationTokenSource();
                cts.CancelAfter(TimeSpan.FromMinutes(1));

                return await tradeEngineConnector.StartTrading(engineName, cross, cts.Token);
            }
            catch (OperationCanceledException)
            {
                string err = "Not requesting to start trading cross: operation cancelled";
                Debug.WriteLine(err);
                return new GenericActionResult(false, err);
            }
            catch (Exception ex)
            {
                string err = $"Failed to request to start trading cross: {ex.Message}";
                Debug.WriteLine(err);
                return new GenericActionResult(false, err);
            }
        }

        public async Task<GenericActionResult> StopTradingStrategy(string engineName, string stratName, string stratVersion)
        {
            try
            {
                CancellationTokenSource cts = new CancellationTokenSource();
                cts.CancelAfter(TimeSpan.FromMinutes(1));

                return await tradeEngineConnector.StopTradingStrategy(engineName, stratName, stratVersion, cts.Token);
            }
            catch (OperationCanceledException)
            {
                string err = "Not requesting to stop trading strategy: operation cancelled";
                Debug.WriteLine(err);
                return new GenericActionResult(false, err);
            }
            catch (Exception ex)
            {
                string err = $"Failed to request to stop trading strategy: {ex.Message}";
                Debug.WriteLine(err);
                return new GenericActionResult(false, err);
            }
        }

        public async Task<GenericActionResult> StartTradingStrategy(string engineName, string stratName, string stratVersion)
        {
            try
            {
                CancellationTokenSource cts = new CancellationTokenSource();
                cts.CancelAfter(TimeSpan.FromMinutes(1));

                return await tradeEngineConnector.StartTradingStrategy(engineName, stratName, stratVersion, cts.Token);
            }
            catch (OperationCanceledException)
            {
                string err = "Not requesting to start trading strategy: operation cancelled";
                Debug.WriteLine(err);
                return new GenericActionResult(false, err);
            }
            catch (Exception ex)
            {
                string err = $"Failed to request to start trading strategy: {ex.Message}";
                Debug.WriteLine(err);
                return new GenericActionResult(false, err);
            }
        }

        public async Task<GenericActionResult> DeactivateStrategy(string engineName, string stratName, string stratVersion)
        {
            try
            {
                CancellationTokenSource cts = new CancellationTokenSource();
                cts.CancelAfter(TimeSpan.FromMinutes(1));

                return await tradeEngineConnector.DeactivateStrategy(engineName, stratName, stratVersion, cts.Token);
            }
            catch (OperationCanceledException)
            {
                string err = "Not requesting to deactivate strategy: operation cancelled";
                Debug.WriteLine(err);
                return new GenericActionResult(false, err);
            }
            catch (Exception ex)
            {
                string err = $"Failed to request to deactivate strategy: {ex.Message}";
                Debug.WriteLine(err);
                return new GenericActionResult(false, err);
            }
        }

        public async Task<GenericActionResult> ActivateStrategy(string engineName, string stratName, string stratVersion)
        {
            try
            {
                CancellationTokenSource cts = new CancellationTokenSource();
                cts.CancelAfter(TimeSpan.FromMinutes(1));

                return await tradeEngineConnector.ActivateStrategy(engineName, stratName, stratVersion, cts.Token);
            }
            catch (OperationCanceledException)
            {
                string err = "Not requesting to activate strategy: operation cancelled";
                Debug.WriteLine(err);
                return new GenericActionResult(false, err);
            }
            catch (Exception ex)
            {
                string err = $"Failed to request to activate strategy: {ex.Message}";
                Debug.WriteLine(err);
                return new GenericActionResult(false, err);
            }
        }

        public async Task<GenericActionResult> ClosePosition(string engineName, string cross)
        {
            try
            {
                CancellationTokenSource cts = new CancellationTokenSource();
                cts.CancelAfter(TimeSpan.FromMinutes(1));

                return await tradeEngineConnector.ClosePosition(engineName, cross, cts.Token);
            }
            catch (OperationCanceledException)
            {
                string err = "Not requesting to close position: operation cancelled";
                Debug.WriteLine(err);
                return new GenericActionResult(false, err);
            }
            catch (Exception ex)
            {
                string err = $"Failed to request to close position: {ex.Message}";
                Debug.WriteLine(err);
                return new GenericActionResult(false, err);
            }
        }

        public async Task<GenericActionResult> CancelOrders(string engineName, string cross)
        {
            try
            {
                CancellationTokenSource cts = new CancellationTokenSource();
                cts.CancelAfter(TimeSpan.FromMinutes(1));

                return await tradeEngineConnector.CancelOrders(engineName, cross, cts.Token);
            }
            catch (OperationCanceledException)
            {
                string err = "Not requesting to cancel orders: operation cancelled";
                Debug.WriteLine(err);
                return new GenericActionResult(false, err);
            }
            catch (Exception ex)
            {
                string err = $"Failed to request to cancel orders: {ex.Message}";
                Debug.WriteLine(err);
                return new GenericActionResult(false, err);
            }
        }
    }
}
