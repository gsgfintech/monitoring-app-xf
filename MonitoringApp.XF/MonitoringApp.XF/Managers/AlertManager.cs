using Capital.GSG.FX.Data.Core.SystemData;
using Capital.GSG.FX.Data.Core.WebApi;
using Capital.GSG.FX.Monitoring.Server.Connector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MonitoringApp.XF.Managers
{
    public class AlertManager
    {
        private readonly BackendAlertsConnector alertsConnector;

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

        private List<Alert> openAlerts;
        private List<Alert> alertsClosedtoday = new List<Alert>();
        private Dictionary<string, Alert> detailedAlerts = new Dictionary<string, Alert>();

        private AlertManager()
        {
            alertsConnector = App.MonitoringServerConnector.AlertsConnector;
        }

        public async Task<List<Alert>> LoadOpenAlerts(bool refresh)
        {
            try
            {
                if (openAlerts == null || refresh)
                {
                    CancellationTokenSource cts = new CancellationTokenSource();
                    cts.CancelAfter(TimeSpan.FromMinutes(1));

                    openAlerts = await alertsConnector.GetByStatus(AlertStatus.OPEN, cts.Token);
                }

                return openAlerts;
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Not retrieving open alerts: operation cancelled");
                return null;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Sync error: {e.Message}");
                return null;
            }
        }

        public async Task<List<Alert>> LoadAlertsClosedToday(bool refresh)
        {
            try
            {
                if (alertsClosedtoday == null || refresh)
                {
                    CancellationTokenSource cts = new CancellationTokenSource();
                    cts.CancelAfter(TimeSpan.FromMinutes(1));

                    var alerts = await alertsConnector.GetForDay(DateTime.Today, cts.Token);
                    alerts = alerts?.Where(a => a.Status == AlertStatus.CLOSED).ToList();

                    if (alerts != null)
                        alertsClosedtoday = alerts;
                    else
                        alertsClosedtoday = new List<Alert>();
                }

                return alertsClosedtoday;
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Not retrieving alerts closed: operation cancelled");
                return null;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Sync error: {e.Message}");
                return null;
            }
        }

        public async Task<Alert> GetAlertById(string id)
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

                    Alert alert = await alertsConnector.GetById(id, cts.Token);

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
            catch (Exception e)
            {
                Debug.WriteLine($"Sync error: {e.Message}");
                return null;
            }
        }

        public async Task<GenericActionResult> CloseAlert(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    throw new ArgumentNullException(nameof(id));

                CancellationTokenSource cts = new CancellationTokenSource();
                cts.CancelAfter(TimeSpan.FromMinutes(1));

                GenericActionResult result = await alertsConnector.Close(id, cts.Token);

                if (result.Success)
                    HandleAlertClosure(id);

                return result;
            }
            catch (OperationCanceledException)
            {
                string err = "Not closing alert: operation cancelled";
                Debug.WriteLine(err);
                return new GenericActionResult(false, err);
            }
            catch (ArgumentNullException ex)
            {
                string err = $"Not closing alert: missing or invalid parameter {ex.ParamName}";
                Debug.WriteLine(err);
                return new GenericActionResult(false, err);
            }
            catch (Exception e)
            {
                string err = $"Failed to close alert: {e.Message}";
                Debug.WriteLine(err);
                return new GenericActionResult(false, err);
            }
        }

        private void HandleAlertClosure(string id)
        {
            Alert alert = openAlerts.FirstOrDefault(a => a.AlertId == id);

            if (alert != null)
            {
                DateTime day = alert.Timestamp.Date;

                alertsClosedtoday.Add(alert);
                openAlerts.Remove(alert);
            }

            detailedAlerts.Remove(id);
        }

        public async Task<GenericActionResult> CloseAllAlerts()
        {
            try
            {
                CancellationTokenSource cts = new CancellationTokenSource();
                cts.CancelAfter(TimeSpan.FromMinutes(1));

                GenericActionResult result = await alertsConnector.CloseAll(cts.Token);

                if (result.Success)
                {
                    foreach (var alert in openAlerts)
                        detailedAlerts.Remove(alert.AlertId);

                    alertsClosedtoday.AddRange(openAlerts);

                    openAlerts.Clear();
                }

                return result;
            }
            catch (OperationCanceledException)
            {
                string err = "Not closing all alerts: operation cancelled";
                Debug.WriteLine(err);
                return new GenericActionResult(false, err);
            }
            catch (Exception e)
            {
                string err = $"Failed to close all alerts: {e.Message}";
                Debug.WriteLine(err);
                return new GenericActionResult(false, err);
            }
        }
    }
}
