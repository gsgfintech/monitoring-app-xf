using Capital.GSG.FX.Data.Core.FinancialAdvisorsData;
using Capital.GSG.FX.Data.Core.OrderData;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonitoringApp.XF.ViewModels
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
                Account = order.Account,
                AllocationInfo = order.AllocationInfo,
                Broker = order.Broker,
                Cross = order.Cross,
                Details = $"{order.PlacedTime:dd/MM/yy HH:mm:ss zzz} | {order.Origin}",
                EstimatedCommission = order.EstimatedCommission,
                EstimatedCommissionCcy = order.EstimatedCommissionCcy,
                FillPrice = order.FillPrice,
                GroupId = order.GroupId,
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

        private static string ComputeAllocationInfo(Order order)
        {
            if (string.IsNullOrEmpty(order.AllocationInfo))
                return "Unknown";

            // 1. Try parse allocation profile
            FAAllocationProfile allocationProfile = null;
            try { allocationProfile = JsonConvert.DeserializeObject<FAAllocationProfile>(order.AllocationInfo); } catch { }

            if (allocationProfile != null)
            {
                StringBuilder sb = new StringBuilder("Allocation Profile:");
                sb.AppendLine($"Name: {allocationProfile.Name}");
                sb.AppendLine($"Type: {allocationProfile.Type}");

                sb.AppendLine("Accounts:");
                foreach (var allocation in allocationProfile.Allocations)
                    sb.AppendLine($"{allocation.Account}:\t{allocation.Amount:N0}");

                return sb.ToString();
            }

            // 2. Try parse FA Group
            FAGroup group = null;
            try { group = JsonConvert.DeserializeObject<FAGroup>(order.AllocationInfo); } catch { }

            if (group != null)
            {
                StringBuilder sb = new StringBuilder("FA Group:");
                sb.AppendLine($"Name: {group.Name}");
                sb.AppendLine($"Method: {group.DefaultMethod}");

                sb.AppendLine("Accounts:");
                foreach (var account in group.Accounts)
                    sb.AppendLine(account);

                return sb.ToString();
            }

            return order.AllocationInfo;
        }
    }
}
