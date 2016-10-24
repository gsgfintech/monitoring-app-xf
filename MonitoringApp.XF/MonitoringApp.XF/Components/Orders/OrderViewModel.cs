using Capital.GSG.FX.Data.Core.OrderData;
using System.Collections.Generic;
using System.Linq;

namespace MonitoringApp.XF.Components.Orders
{
    public class OrderViewModel : Order
    {
        public string Header { get { return $"{Side} {Quantity / 1000:N0}K {Cross} ({Utils.ShortenOrderType(Type)})"; } }

        public string Details { get; set; }
    }

    public static class OrderViewModelExtensions
    {
        public static OrderViewModel ToOrderViewModel(this Order order)
        {
            if (order == null)
                return null;

            return new OrderViewModel()
            {
                Cross = order.Cross,
                Details = $"{order.PlacedTime:dd/MM/yy HH:mm:ss zzz} | {order.Origin}",
                EstimatedCommission = order.EstimatedCommission,
                EstimatedCommissionCcy = order.EstimatedCommissionCcy,
                FillPrice = order.FillPrice,
                History = order.History,
                LastAsk = order.LastAsk,
                LastBid = order.LastBid,
                LastMid = order.LastMid,
                LastUpdateTime = order.LastUpdateTime,
                LimitPrice = order.LimitPrice,
                OrderID = order.OrderID,
                Origin = order.Origin,
                OurRef = order.OurRef,
                ParentOrderID = order.ParentOrderID,
                PermanentID = order.PermanentID,
                PlacedTime = order.PlacedTime,
                Quantity = order.Quantity,
                Side = order.Side,
                Status = order.Status,
                StopPrice = order.StopPrice,
                Strategy = order.Strategy,
                TimeInForce = order.TimeInForce,
                TrailingAmount = order.TrailingAmount,
                Type = order.Type,
                UsdQuantity = order.UsdQuantity
            };
        }

        public static IEnumerable<OrderViewModel> ToOrderViewModels(this IEnumerable<Order> orders)
        {
            return orders?.Select(o => o.ToOrderViewModel());
        }
    }
}
