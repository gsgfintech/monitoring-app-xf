using Capital.GSG.FX.Monitoring.AppDataTypes;
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

        private List<ExecutionSlim> todaysExecutions;
        private Dictionary<string, ExecutionFull> detailedExecutions = new Dictionary<string, ExecutionFull>();

        private ExecutionManager()
        {
            client = App.MobileServiceClient;
        }

        public async Task<List<ExecutionSlim>> LoadTodaysExecutions(bool refresh)
        {
            try
            {
                if (todaysExecutions == null || refresh)
                {
                    CancellationTokenSource cts = new CancellationTokenSource();
                    cts.CancelAfter(TimeSpan.FromMinutes(1));

                    todaysExecutions = await client.InvokeApiAsync<List<ExecutionSlim>>(ExecutionsRoute, HttpMethod.Get, null, cts.Token);
                }

                return todaysExecutions;
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Not retrieving today's executions: operation cancelled");
                return null;
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                if (msioe.Response?.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Debug.WriteLine("Authentication necessary to load today's executions");
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

        public async Task<ExecutionFull> GetExecutionById(string id)
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

                    ExecutionFull execution = await client.InvokeApiAsync<ExecutionFull>($"{ExecutionsRoute}", HttpMethod.Get, new Dictionary<string, string>() { { "executionId", id } }, cts.Token);

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
