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

namespace MonitoringApp.XF.Managers
{
    public class SystemStatusManager
    {
        private readonly BackendSystemServicesConnector systemServicesConnector;
        private readonly BackendSystemStatusesConnector systemStatusesConnector;

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
            systemServicesConnector = App.MonitoringServerConnector.SystemServicesConnector;
            systemStatusesConnector = App.MonitoringServerConnector.SystemStatusesConnector;
        }

        public async Task<List<SystemStatus>> LoadSystemsStatuses(bool refresh)
        {
            try
            {
                if (statusDict == null || refresh)
                {
                    CancellationTokenSource cts = new CancellationTokenSource();
                    cts.CancelAfter(TimeSpan.FromMinutes(1));

                    var statuses = await systemStatusesConnector.GetAll(cts.Token);

                    if (!statuses.IsNullOrEmpty())
                    {
                        foreach (var status in statuses)
                        {
                            if (!status.IsAlive)
                                status.OverallStatus = SystemStatusLevel.RED;
                            else if (!status.OverallStatus.HasValue)
                            {
                                if (!status.Attributes.IsNullOrEmpty())
                                    status.OverallStatus = SystemStatusLevelUtils.CalculateWorstOf(status.Attributes.Select(a => a.Level));
                                else
                                    status.OverallStatus = SystemStatusLevel.GREEN;
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

                    SystemStatus status = await systemStatusesConnector.Get(name, cts.Token);

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

                return await systemServicesConnector.StartService(name, cts.Token);
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

                return await systemServicesConnector.StopService(name, cts.Token);
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
            catch (Exception e)
            {
                Debug.WriteLine($"Failed to stop system: {e.Message}");
                return null;
            }
        }
    }
}
