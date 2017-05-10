using Capital.GSG.FX.Data.Core.AccountPortfolio;
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
        private Dictionary<(Broker Broker, int PermanentId), Order> detailedOrders = new Dictionary<(Broker Broker, int PermanentId), Order>();

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

                    var result = await ordersConnector.GetOrdersForDay(day, cts.Token);

                    if (result.Success && !result.Orders.IsNullOrEmpty())
                        ordersDict[day] = result.Orders;
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

        public async Task<Order> GetOrderByPermanentId(Broker broker, int permanentId)
        {
            try
            {
                if (permanentId <= 0)
                    throw new ArgumentNullException(nameof(permanentId));

                if (detailedOrders.ContainsKey((broker, permanentId)) && detailedOrders[(broker, permanentId)] != null)
                    return detailedOrders[(broker, permanentId)];
                else
                {
                    CancellationTokenSource cts = new CancellationTokenSource();
                    cts.CancelAfter(TimeSpan.FromMinutes(1));

                    var result = await ordersConnector.Get(broker, permanentId, cts.Token);

                    if (result.Success && result.Order != null)
                        detailedOrders[(broker, permanentId)] = result.Order;

                    return result.Order;
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
