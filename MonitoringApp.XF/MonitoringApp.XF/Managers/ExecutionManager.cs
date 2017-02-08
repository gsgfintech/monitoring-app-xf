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
        private Dictionary<string, Execution> detailedExecutions = new Dictionary<string, Execution>();

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

                    var executions = await executionsConnector.GetForDay(day, cts.Token);

                    if (!executions.IsNullOrEmpty())
                        executionsDict[day] = executions;
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

        public async Task<Execution> GetExecutionById(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    throw new ArgumentNullException(nameof(id));

                if (detailedExecutions.ContainsKey(id) && detailedExecutions[id] != null)
                    return detailedExecutions[id];
                else
                {
                    CancellationTokenSource cts = new CancellationTokenSource();
                    cts.CancelAfter(TimeSpan.FromMinutes(1));

                    Execution execution = await executionsConnector.GetById(id, cts.Token);

                    if (execution != null)
                        detailedExecutions[id] = execution;

                    return execution;
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
