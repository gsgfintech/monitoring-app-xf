using Capital.GSG.FX.Monitoring.AppDataTypes;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MonitoringApp.XF.Components.Alerts
{
    public class AlertManager
    {
        private const string AlertsOpenRoute = "alerts/list/open";
        private const string AlertsClosedTodayRoute = "alerts/list/closedtoday";
        private const string AlertByIdRoute = "alerts/id";
        private const string CloseAlertRoute = "alerts/close";

        private readonly MobileServiceClient client;

        private static AlertManager instance;
        public static AlertManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new AlertManager();

                return instance;
            }
        }

        private List<AlertSlim> openAlerts;
        private List<AlertSlim> alertsClosedToday;
        private Dictionary<string, AlertFull> detailedAlerts = new Dictionary<string, AlertFull>();

        private AlertManager()
        {
            client = App.MobileServiceClient;
        }

        public async Task<List<AlertSlim>> LoadOpenAlerts(bool refresh)
        {
            try
            {
                if (openAlerts == null || refresh)
                {
                    CancellationTokenSource cts = new CancellationTokenSource();
                    cts.CancelAfter(TimeSpan.FromMinutes(1));

                    openAlerts = await client.InvokeApiAsync<List<AlertSlim>>(AlertsOpenRoute, HttpMethod.Get, null, cts.Token);
                }

                return openAlerts;
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Not retrieving open alerts: operation cancelled");
                return null;
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                if (msioe.Response?.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Debug.WriteLine("Authentication necessary to load open alerts");
                    throw new AuthenticationRequiredException(typeof(AlertManager)); // Re-throw the unauthorized exception and catch it in the VM to redirect to the login page
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

        public async Task<List<AlertSlim>> LoadAlertsClosedToday(bool refresh)
        {
            try
            {
                if (alertsClosedToday == null || refresh)
                {
                    CancellationTokenSource cts = new CancellationTokenSource();
                    cts.CancelAfter(TimeSpan.FromMinutes(1));

                    alertsClosedToday = await client.InvokeApiAsync<List<AlertSlim>>(AlertsClosedTodayRoute, HttpMethod.Get, null, cts.Token);
                }

                return alertsClosedToday;
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Not retrieving alerts closed today: operation cancelled");
                return null;
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                if (msioe.Response?.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Debug.WriteLine("Authentication necessary to load alerts closed today");
                    throw new AuthenticationRequiredException(typeof(AlertManager)); // Re-throw the unauthorized exception and catch it in the VM to redirect to the login page
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

        public async Task<AlertFull> GetAlertById(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    throw new ArgumentNullException(nameof(id));

                if (detailedAlerts.ContainsKey(id) && detailedAlerts[id] != null)
                    return detailedAlerts[id];
                else
                {
                    CancellationTokenSource cts = new CancellationTokenSource();
                    cts.CancelAfter(TimeSpan.FromMinutes(1));

                    AlertFull alert = await client.InvokeApiAsync<AlertFull>($"{AlertByIdRoute}/{id}", HttpMethod.Get, new Dictionary<string, string>() { { "executionId", id } }, cts.Token);

                    if (alert != null)
                        detailedAlerts[id] = alert;

                    return alert;
                }
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Not retrieving alert's details: operation cancelled");
                return null;
            }
            catch (ArgumentNullException ex)
            {
                Debug.WriteLine($"Not retrieving alert's details: missing or invalid parameter {ex.ParamName}");
                return null;
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                if (msioe.Response?.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Debug.WriteLine("Authentication necessary to load alert");
                    throw new AuthenticationRequiredException(typeof(AlertManager)); // Re-throw the unauthorized exception and catch it in the VM to redirect to the login page
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

        public async Task<bool> CloseAlert(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    throw new ArgumentNullException(nameof(id));

                CancellationTokenSource cts = new CancellationTokenSource();
                cts.CancelAfter(TimeSpan.FromMinutes(1));

                bool success = await client.InvokeApiAsync<bool>($"{CloseAlertRoute}/{id}", HttpMethod.Get, null, cts.Token);

                if (success)
                {
                    AlertSlim alert = openAlerts.FirstOrDefault(a => a.Id == id);

                    if (alert != null)
                    {
                        if (alertsClosedToday == null)
                            alertsClosedToday = new List<AlertSlim>();

                        alertsClosedToday.Add(alert);
                        openAlerts.Remove(alert);
                    }

                    detailedAlerts.Remove(id);
                }

                return success;
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Not closing alert: operation cancelled");
                return false;
            }
            catch (ArgumentNullException ex)
            {
                Debug.WriteLine($"Not closing alert: missing or invalid parameter {ex.ParamName}");
                return false;
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                if (msioe.Response?.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Debug.WriteLine("Authentication necessary to close alert");
                    throw new AuthenticationRequiredException(typeof(AlertManager)); // Re-throw the unauthorized exception and catch it in the VM to redirect to the login page
                }

                Debug.WriteLine($"Failed to close alert: {msioe.Message}");
                return false;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Failed to close alert: {e.Message}");
                return false;
            }
        }

        public async Task<bool> CloseAllAlerts()
        {
            try
            {
                CancellationTokenSource cts = new CancellationTokenSource();
                cts.CancelAfter(TimeSpan.FromMinutes(1));

                bool success = await client.InvokeApiAsync<bool>($"{CloseAlertRoute}/all", HttpMethod.Get, null, cts.Token);

                if (success)
                {
                    if (alertsClosedToday == null)
                        alertsClosedToday = new List<AlertSlim>();

                    alertsClosedToday.AddRange(openAlerts);

                    foreach (var alert in openAlerts)
                        detailedAlerts.Remove(alert.Id);

                    openAlerts.Clear();
                }

                return success;
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Not closing all alerts: operation cancelled");
                return false;
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                if (msioe.Response?.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Debug.WriteLine("Authentication necessary to close all alerts");
                    throw new AuthenticationRequiredException(typeof(AlertManager)); // Re-throw the unauthorized exception and catch it in the VM to redirect to the login page
                }

                Debug.WriteLine($"Failed to close all alerts: {msioe.Message}");
                return false;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Failed to close all alerts: {e.Message}");
                return false;
            }
        }
    }
}
