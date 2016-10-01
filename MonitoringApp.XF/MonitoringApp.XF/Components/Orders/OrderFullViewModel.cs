using Capital.GSG.FX.Monitoring.AppDataTypes;

namespace MonitoringApp.XF.Components.Orders
{
    public class OrderFullViewModel : OrderFull
    {
        public string Header { get { return $"{Side} {Quantity / 1000:N0}K {Cross} ({Utils.ShortenOrderType(Type)})"; } }
    }

    public static class OrderFullViewModelExtensions
    {
        public static OrderFullViewModel ToOrderFullViewModel(this OrderFull order)
        {
            if (order == null)
                return null;

            return new OrderFullViewModel()
            {
                ClientId = order.ClientId,
                ContractId = order.ContractId,
                Cross = order.Cross,
                EstimatedCommission = order.EstimatedCommission,
                EstimatedCommissionCcy = order.EstimatedCommissionCcy,
                ExitProfitabilityLevel = order.ExitProfitabilityLevel,
                FillPrice = order.FillPrice,
                History = order.History,
                LastAsk = order.LastAsk,
                LastBid = order.LastBid,
                LastMid = order.LastMid,
                LastUpdateTime = order.LastUpdateTime,
                LimitPrice = order.LimitPrice,
                OrderId = order.OrderId,
                Origin = order.Origin,
                OurRef = order.OurRef,
                ParentOrderId = order.ParentOrderId,
                PermanentId = order.PermanentId,
                PlacedTime = order.PlacedTime,
                Quantity = order.Quantity,
                Side = order.Side,
                Status = order.Status,
                StopPrice = order.StopPrice,
                Strategy = order.Strategy,
                TimeInForce = order.TimeInForce,
                TrailingAmount = order.TrailingAmount,
                TransmitOrder = order.TransmitOrder,
                Type = order.Type,
                UsdQuantity = order.UsdQuantity,
                WarningMessage = order.WarningMessage
            };
        }
    }
}
