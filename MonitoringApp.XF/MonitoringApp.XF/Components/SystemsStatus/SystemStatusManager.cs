using Capital.GSG.FX.Data.Core.SystemData;
using Capital.GSG.FX.Utils.Portable;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MonitoringApp.XF.Components.SystemsStatus
{
    public class SystemStatusManager
    {
        private const string SystemsRoute = "systems";

        private readonly MobileServiceClient client;

        private Dictionary<string, SystemStatus> statusDict;

        private static SystemStatusManager instance;
        public static SystemStatusManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new SystemStatusManager();

                return instance;
            }
        }

        private SystemStatusManager()
        {
            client = App.MobileServiceClient;
        }

        public async Task<List<SystemStatus>> LoadSystemsStatuses(bool refresh)
        {
            try
            {
                if (statusDict == null || refresh)
                {
                    CancellationTokenSource cts = new CancellationTokenSource();
                    cts.CancelAfter(TimeSpan.FromMinutes(1));

                    var statuses = await client.InvokeApiAsync<List<SystemStatus>>($"{SystemsRoute}", HttpMethod.Get, null, cts.Token);

                    if (!statuses.IsNullOrEmpty())
                    {
                        foreach (var status in statuses)
                        {
                            if (!status.OverallStatus.HasValue)
                            {
                                if (!status.Attributes.IsNullOrEmpty())
                                    status.OverallStatus = SystemStatusLevelUtils.CalculateWorstOf(status.Attributes.Select(a => a.Level));
                                else
                                    status.OverallStatus = SystemStatusLevel.RED;
                            }
                        }

                        statusDict = statuses.ToDictionary(s => s.Name, s => s);
                    }
                    else
                        statusDict = new Dictionary<string, SystemStatus>();
                }

                return statusDict.Values.ToList();
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Not retrieving systems statuses: operation cancelled");
                return null;
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                if (msioe.Response?.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Debug.WriteLine("Authentication necessary to load systems statuses");
                    throw new AuthenticationRequiredException(typeof(SystemStatusManager)); // Re-throw the unauthorized exception and catch it in the VM to redirect to the login page
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

        public async Task<SystemStatus> GetSystemStatusByName(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                    throw new ArgumentNullException(nameof(name));

                if (statusDict != null && statusDict.ContainsKey(name) && statusDict[name] != null)
                    return statusDict[name];
                else
                {
                    CancellationTokenSource cts = new CancellationTokenSource();
                    cts.CancelAfter(TimeSpan.FromMinutes(1));

                    SystemStatus status = await client.InvokeApiAsync<SystemStatus>($"{SystemsRoute}/{name}", HttpMethod.Get, null, cts.Token);

                    if (status != null)
                    {
                        if (statusDict == null)
                            statusDict = new Dictionary<string, SystemStatus>();

                        statusDict[name] = status;
                    }

                    return status;
                }
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Not retrieving system status details: operation cancelled");
                return null;
            }
            catch (ArgumentNullException ex)
            {
                Debug.WriteLine($"Not retrieving system status: missing or invalid parameter {ex.ParamName}");
                return null;
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                if (msioe.Response?.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Debug.WriteLine("Authentication necessary to system status");
                    throw new AuthenticationRequiredException(typeof(SystemStatusManager)); // Re-throw the unauthorized exception and catch it in the VM to redirect to the login page
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

        public async Task<GenericActionResult> StartSystem(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                    throw new ArgumentNullException(nameof(name));

                CancellationTokenSource cts = new CancellationTokenSource();
                cts.CancelAfter(TimeSpan.FromMinutes(1));

                return await client.InvokeApiAsync<GenericActionResult>($"{SystemsRoute}/{name}/start", HttpMethod.Get, null, cts.Token);
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Not starting system: operation cancelled");
                return null;
            }
            catch (ArgumentNullException ex)
            {
                Debug.WriteLine($"Not starting system: missing or invalid parameter {ex.ParamName}");
                return null;
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                if (msioe.Response?.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Debug.WriteLine("Authentication necessary to start system");
                    throw new AuthenticationRequiredException(typeof(SystemStatusManager)); // Re-throw the unauthorized exception and catch it in the VM to redirect to the login page
                }

                Debug.WriteLine($"Failed to start system: {msioe.Message}");
                return null;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Failed to start system: {e.Message}");
                return null;
            }
        }

        public async Task<GenericActionResult> StopSystem(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                    throw new ArgumentNullException(nameof(name));

                CancellationTokenSource cts = new CancellationTokenSource();
                cts.CancelAfter(TimeSpan.FromMinutes(1));

                return await client.InvokeApiAsync<GenericActionResult>($"{SystemsRoute}/{name}/stop", HttpMethod.Get, null, cts.Token);
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Not stopping system: operation cancelled");
                return null;
            }
            catch (ArgumentNullException ex)
            {
                Debug.WriteLine($"Not stopping system: missing or invalid parameter {ex.ParamName}");
                return null;
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                if (msioe.Response?.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Debug.WriteLine("Authentication necessary to start system");
                    throw new AuthenticationRequiredException(typeof(SystemStatusManager)); // Re-throw the unauthorized exception and catch it in the VM to redirect to the login page
                }

                Debug.WriteLine($"Failed to stop system: {msioe.Message}");
                return null;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Failed to stop system: {e.Message}");
                return null;
            }
        }
    }
}
