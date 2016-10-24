using Capital.GSG.FX.Data.Core.ExecutionData;
using Capital.GSG.FX.Utils.Portable;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MonitoringApp.XF.Components.Executions
{
    public class ExecutionManager
    {
        private const string ExecutionsRoute = "executions";

        private readonly MobileServiceClient client;

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
            client = App.MobileServiceClient;
        }

        public async Task<List<Execution>> LoadExecutions(DateTime day, bool refresh)
        {
            try
            {
                if (!executionsDict.ContainsKey(day) || executionsDict[day] == null || refresh)
                {
                    CancellationTokenSource cts = new CancellationTokenSource();
                    cts.CancelAfter(TimeSpan.FromMinutes(1));

                    var executions = await client.InvokeApiAsync<List<Execution>>(ExecutionsRoute, HttpMethod.Get, new Dictionary<string, string>() {
                        { "day", day.ToString("yyyy-MM-dd") }
                    }, cts.Token);

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
            catch (MobileServiceInvalidOperationException msioe)
            {
                if (msioe.Response?.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Debug.WriteLine("Authentication necessary to load executions");
                    throw new AuthenticationRequiredException(typeof(ExecutionManager)); // Re-throw the unauthorized exception and catch it in the VM to redirect to the login page
                }

                Debug.WriteLine($"Invalid sync operation: {msioe.Message}");
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

                    Execution execution = await client.InvokeApiAsync<Execution>($"{ExecutionsRoute}", HttpMethod.Get, new Dictionary<string, string>() { { "executionId", id } }, cts.Token);

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
            catch (MobileServiceInvalidOperationException msioe)
            {
                if (msioe.Response?.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Debug.WriteLine("Authentication necessary to load execution");
                    throw new AuthenticationRequiredException(typeof(ExecutionManager)); // Re-throw the unauthorized exception and catch it in the VM to redirect to the login page
                }

                Debug.WriteLine($"Invalid sync operation: {msioe.Message}");
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
