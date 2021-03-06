﻿using Capital.GSG.FX.Data.Core.AccountPortfolio;
using Capital.GSG.FX.Data.Core.ExecutionData;
using Capital.GSG.FX.Monitoring.Server.Connector;
using Capital.GSG.FX.Utils.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace MonitoringApp.XF.Managers
{
    public class ExecutionManager
    {
        private readonly BackendExecutionsConnector executionsConnector;

        private static ExecutionManager instance;
        public static ExecutionManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new ExecutionManager();

                return instance;
            }
        }

        private Dictionary<DateTime, List<Execution>> executionsDict = new Dictionary<DateTime, List<Execution>>();
        private Dictionary<(Broker Broker, string Id), Execution> detailedExecutions = new Dictionary<(Broker Broker, string Id), Execution>();

        private ExecutionManager()
        {
            executionsConnector = App.MonitoringServerConnector.ExecutionsConnector;
        }

        public async Task<List<Execution>> LoadExecutions(DateTime day, bool refresh)
        {
            try
            {
                if (!executionsDict.ContainsKey(day) || executionsDict[day] == null || refresh)
                {
                    CancellationTokenSource cts = new CancellationTokenSource();
                    cts.CancelAfter(TimeSpan.FromMinutes(1));

                    var executionsResult = await executionsConnector.GetForDay(Broker.IB, day, cts.Token);

                    if (executionsResult.Success && !executionsResult.Trades.IsNullOrEmpty())
                        executionsDict[day] = executionsResult.Trades;
                    else
                        executionsDict[day] = new List<Execution>();
                }

                return executionsDict[day];
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Not retrieving executions: operation cancelled");
                return null;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Failed to load executions: {e.Message}");
                return null;
            }
        }

        public async Task<Execution> GetExecutionById(Broker broker, string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    throw new ArgumentNullException(nameof(id));

                if (detailedExecutions.ContainsKey((broker, id)) && detailedExecutions[(broker, id)] != null)
                    return detailedExecutions[(broker, id)];
                else
                {
                    CancellationTokenSource cts = new CancellationTokenSource();
                    cts.CancelAfter(TimeSpan.FromMinutes(1));

                    var result = await executionsConnector.GetById(broker, id, cts.Token);

                    if (result.Success && result.Trade != null)
                        detailedExecutions[(broker, id)] = result.Trade;

                    return result.Trade;
                }
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Not retrieving execution's details: operation cancelled");
                return null;
            }
            catch (ArgumentNullException ex)
            {
                Debug.WriteLine($"Not retrieving execution's details: missing or invalid parameter {ex.ParamName}");
                return null;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Sync error: {e.Message}");
                return null;
            }
        }
    }
}
