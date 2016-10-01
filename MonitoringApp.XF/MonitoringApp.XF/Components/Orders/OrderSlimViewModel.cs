using Capital.GSG.FX.Monitoring.AppDataTypes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MonitoringApp.XF.Components.Orders
{
    public class OrderSlimViewModel : OrderSlim
    {
        public string Header { get { return $"{Side} {Quantity / 1000:N0}K {Cross} ({Utils.ShortenOrderType(Type)})"; } }

        public string Details { get; set; }
    }

    public static class OrderSlimViewModelExentsions
    {
        private static OrderSlimViewModel ToOrderSlimViewModel(this OrderSlim order)
        {
            if (order == null)
                return null;

            return new OrderSlimViewModel()
            {
                Cross = order.Cross,
                Details = $"{order.PlacedTime:dd/MM/yy HH:mm:ss zzz} | {order.Origin}",
                FillPrice = order.FillPrice,
                OrderId = order.OrderId,
                Origin = order.Origin,
                PermanentId = order.PermanentId,
                PlacedTime = order.PlacedTime,
                Quantity = order.Quantity,
                Side = order.Side,
                Status = order.Status,
                Type = order.Type
            };
        }

        public static IEnumerable<OrderSlimViewModel> ToOrderSlimViewModels(this IEnumerable<OrderSlim> orders)
        {
            return orders?.Select(o => o.ToOrderSlimViewModel());
        }
    }

    public class GroupedOrdersList : ObservableCollection<OrderSlimViewModel>
    {
        public string LongDescription { get; private set; }
        public string ShortDescription { get; private set; }

        public GroupedOrdersList(string longDescription, string shortDescription)
        {
            LongDescription = longDescription;
            ShortDescription = shortDescription;
        }

        public GroupedOrdersList(string longDescription, string shortDescription, IEnumerable<OrderSlim> orders)
            : base(orders.ToOrderSlimViewModels())
        {
            LongDescription = longDescription;
            ShortDescription = shortDescription;
        }
    }
}
