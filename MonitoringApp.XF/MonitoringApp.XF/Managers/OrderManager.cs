using Capital.GSG.FX.Data.Core.OrderData;
using Capital.GSG.FX.Monitoring.Server.Connector;
using Capital.GSG.FX.Utils.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace MonitoringApp.XF.Managers
{
    public class OrderManager
    {
        private const string OrdersRoute = "orders";

        private readonly BackendOrdersConnector ordersConnector;

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

        private Dictionary<DateTime, List<Order>> ordersDict = new Dictionary<DateTime, List<Order>>();
        private Dictionary<int, Order> detailedOrders = new Dictionary<int, Order>();

        private OrderManager()
        {
            ordersConnector = App.MonitoringServerConnector.OrdersConnector;
        }

        public async Task<List<Order>> LoadOrders(DateTime day, bool refresh)
        {
            try
            {
                if (!ordersDict.ContainsKey(day) || ordersDict[day] == null || refresh)
                {
                    CancellationTokenSource cts = new CancellationTokenSource();
                    cts.CancelAfter(TimeSpan.FromMinutes(1));

                    var orders = await ordersConnector.GetOrdersForDay(day, cts.Token);

                    if (!orders.IsNullOrEmpty())
                        ordersDict[day] = orders;
                    else
                        ordersDict[day] = new List<Order>();
                }

                return ordersDict[day];
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Not retrieving orders: operation cancelled");
                return null;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Failed to load orders: {e.Message}");
                return null;
            }
        }

        public async Task<Order> GetOrderByPermanentId(int permanentId)
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

                    Order order = await ordersConnector.GetOrderByPermanentId(permanentId, cts.Token);

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
            catch (Exception e)
            {
                Debug.WriteLine($"Sync error: {e.Message}");
                return null;
            }
        }
    }
}
