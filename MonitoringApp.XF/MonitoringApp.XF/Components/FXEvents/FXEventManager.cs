using Capital.GSG.FX.Data.Core.MarketData;
using Capital.GSG.FX.Monitoring.Server.Connector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace MonitoringApp.XF.Components.FXEvents
{
    public class FXEventManager
    {
        private readonly BackendFXEventsConnector fxEventsConnector;

        private List<FXEvent> todaysHighImpactFXEvents;
        private List<FXEvent> currentWeeksFXEvents;
        private Dictionary<string, FXEvent> detailedFXEvents = new Dictionary<string, FXEvent>();

        private static FXEventManager instance;
        public static FXEventManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new FXEventManager();

                return instance;
            }
        }

        private FXEventManager()
        {
            fxEventsConnector = App.MonitoringServerConnector.FXEventsConnector;
        }

        public async Task<List<FXEvent>> LoadTodaysHighImpactFXEvents(bool refresh)
        {
            try
            {
                if (todaysHighImpactFXEvents == null || refresh)
                {
                    CancellationTokenSource cts = new CancellationTokenSource();
                    cts.CancelAfter(TimeSpan.FromMinutes(1));

                    todaysHighImpactFXEvents = await fxEventsConnector.GetHighImpactForToday(cts.Token);
                }

                return todaysHighImpactFXEvents;
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Not retrieving today's high impact FX events: operation cancelled");
                return null;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Sync error: {e.Message}");
                return null;
            }
        }

        public async Task<List<FXEvent>> LoadCurrentWeeksFXEvents(bool refresh)
        {
            try
            {
                if (currentWeeksFXEvents == null || refresh)
                {
                    CancellationTokenSource cts = new CancellationTokenSource();
                    cts.CancelAfter(TimeSpan.FromMinutes(1));

                    currentWeeksFXEvents = await fxEventsConnector.GetForCurrentWeek(cts.Token);
                }

                return currentWeeksFXEvents;
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Not retrieving current week's FX events: operation cancelled");
                return null;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Sync error: {e.Message}");
                return null;
            }
        }

        public async Task<FXEvent> GetFXEventById(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    throw new ArgumentNullException(nameof(id));

                if (detailedFXEvents.ContainsKey(id) && detailedFXEvents[id] != null)
                    return detailedFXEvents[id];
                else
                {
                    CancellationTokenSource cts = new CancellationTokenSource();
                    cts.CancelAfter(TimeSpan.FromMinutes(1));

                    FXEvent fxEvent = await fxEventsConnector.GetById(id, cts.Token);

                    if (fxEvent != null)
                        detailedFXEvents[id] = fxEvent;

                    return fxEvent;
                }
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Not retrieving FX event's details: operation cancelled");
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
