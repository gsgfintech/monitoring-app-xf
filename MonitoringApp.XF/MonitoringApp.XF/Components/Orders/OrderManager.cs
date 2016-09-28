using Capital.GSG.FX.Monitoring.AppDataTypes;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MonitoringApp.XF.Components.Orders
{
    public class OrderManager
    {
        private const string OrdersRoute = "orders";

        private readonly MobileServiceClient client;

        private static OrderManager instance;
        public static OrderManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new OrderManager();

                return instance;
            }
        }

        private List<OrderSlim> todaysOrders;
        private Dictionary<int, OrderFull> detailedOrders = new Dictionary<int, OrderFull>();

        private OrderManager()
        {
            client = App.MobileServiceClient;
        }

        public async Task<List<OrderSlim>> LoadTodaysOrders(bool refresh)
        {
            try
            {
                if (todaysOrders == null || refresh)
                {
                    CancellationTokenSource cts = new CancellationTokenSource();
                    cts.CancelAfter(TimeSpan.FromMinutes(1));

                    todaysOrders = await client.InvokeApiAsync<List<OrderSlim>>(OrdersRoute, HttpMethod.Get, null, cts.Token);
                }

                return todaysOrders;
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Not retrieving today's orders: operation cancelled");
                return null;
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                if (msioe.Response?.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Debug.WriteLine("Authentication necessary to load today's orders");
                    throw new AuthenticationRequiredException(typeof(OrderManager)); // Re-throw the unauthorized exception and catch it in the VM to redirect to the login page
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

        public async Task<OrderFull> GetOrderByPermanentId(int permanentId)
        {
            try
            {
                if (permanentId <= 0)
                    throw new ArgumentNullException(nameof(permanentId));

                if (detailedOrders.ContainsKey(permanentId) && detailedOrders[permanentId] != null)
                    return detailedOrders[permanentId];
                else
                {
                    CancellationTokenSource cts = new CancellationTokenSource();
                    cts.CancelAfter(TimeSpan.FromMinutes(1));

                    OrderFull order = await client.InvokeApiAsync<OrderFull>($"{OrdersRoute}", HttpMethod.Get, new Dictionary<string, string>() { { "permanentId", permanentId.ToString() } }, cts.Token);

                    if (order != null)
                        detailedOrders[permanentId] = order;

                    return order;
                }
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Not retrieving order's details: operation cancelled");
                return null;
            }
            catch (ArgumentNullException ex)
            {
                Debug.WriteLine($"Not retrieving order's details: missing or invalid parameter {ex.ParamName}");
                return null;
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                if (msioe.Response?.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Debug.WriteLine("Authentication necessary to load order");
                    throw new AuthenticationRequiredException(typeof(OrderManager)); // Re-throw the unauthorized exception and catch it in the VM to redirect to the login page
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
