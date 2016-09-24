﻿using Capital.GSG.FX.Monitoring.AppDataTypes;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MonitoringApp.XF.Components.FXEvents
{
    public class FXEventManager
    {
        private const string FXEventsRoute = "fxevents";

        private readonly MobileServiceClient client;

        private List<FXEventSlim> todaysHighImpactFXEvents;
        private List<FXEventSlim> currentWeeksFXEvents;
        private Dictionary<string, FXEventFull> detailedFXEvents = new Dictionary<string, FXEventFull>();

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
            client = App.MobileServiceClient;
        }

        public async Task<List<FXEventSlim>> LoadTodaysHighImpactFXEvents(bool refresh)
        {
            try
            {
                if (todaysHighImpactFXEvents == null || refresh)
                {
                    CancellationTokenSource cts = new CancellationTokenSource();
                    cts.CancelAfter(TimeSpan.FromMinutes(1));

                    todaysHighImpactFXEvents = await client.InvokeApiAsync<List<FXEventSlim>>($"{FXEventsRoute}/list/todayhigh", HttpMethod.Get, null, cts.Token);
                }

                return todaysHighImpactFXEvents;
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Not retrieving today's high impact FX events: operation cancelled");
                return null;
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                if (msioe.Response?.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Debug.WriteLine("Authentication necessary to load today's high impact FX events");
                    throw new AuthenticationRequiredException(typeof(FXEventManager)); // Re-throw the unauthorized exception and catch it in the VM to redirect to the login page
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

        public async Task<List<FXEventSlim>> LoadCurrentWeeksFXEvents(bool refresh)
        {
            try
            {
                if (currentWeeksFXEvents == null || refresh)
                {
                    CancellationTokenSource cts = new CancellationTokenSource();
                    cts.CancelAfter(TimeSpan.FromMinutes(1));

                    currentWeeksFXEvents = await client.InvokeApiAsync<List<FXEventSlim>>($"{FXEventsRoute}/list/week", HttpMethod.Get, null, cts.Token);
                }

                return currentWeeksFXEvents;
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Not retrieving current week's FX events: operation cancelled");
                return null;
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                if (msioe.Response?.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Debug.WriteLine("Authentication necessary to current week's FX events");
                    throw new AuthenticationRequiredException(typeof(FXEventManager)); // Re-throw the unauthorized exception and catch it in the VM to redirect to the login page
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

        public async Task<FXEventFull> GetFXEventById(string id)
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

                    FXEventFull fxEvent = await client.InvokeApiAsync<FXEventFull>($"{FXEventsRoute}/id/{id}", HttpMethod.Get, null, cts.Token);

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
            catch (MobileServiceInvalidOperationException msioe)
            {
                if (msioe.Response?.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Debug.WriteLine("Authentication necessary to load FX event");
                    throw new AuthenticationRequiredException(typeof(FXEventManager)); // Re-throw the unauthorized exception and catch it in the VM to redirect to the login page
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
