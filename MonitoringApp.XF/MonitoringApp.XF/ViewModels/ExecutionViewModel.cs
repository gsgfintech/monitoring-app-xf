using Capital.GSG.FX.Data.Core.ExecutionData;
using System.Text;

namespace MonitoringApp.XF.ViewModels
{
    public class ExecutionViewModel : Execution
    {
        public string Header { get { return $"{Side} {Quantity / 1000:N0}K {Cross} @ {Price}"; } }

        public string Details { get; set; }

        public string FormattedCommission { get { return $"{Commission:N2} {CommissionCcy}"; } }
    }

    public static class ExecutionViewModelExtensions
    {
        public static ExecutionViewModel ToExecutionViewModel(this Execution execution)
        {
            if (execution == null)
                return null;

            StringBuilder details = new StringBuilder($"{execution.ExecutionTime:dd/MM/yy HH:mm:ss} | {Utils.ShortenOrderOrigin(execution.OrderOrigin)}");

            if (execution.RealizedPnlUsd.HasValue)
            {
                string prefix = execution.RealizedPnlUsd.Value >= 0 ? "+" : "";
                details.Append($" | {prefix}{execution.RealizedPnlUsd:N0} USD");
            }

            if (!string.IsNullOrEmpty(execution.TradeDuration))
                details.Append($" | {execution.TradeDuration}");

            return new ExecutionViewModel()
            {
                AccountNumber = execution.AccountNumber,
                ClientId = execution.ClientId,
                ClientOrderRef = execution.ClientOrderRef,
                Commission = execution.Commission,
                CommissionCcy = execution.CommissionCcy,
                CommissionUsd = execution.CommissionUsd,
                Cross = execution.Cross,
                Details = details.ToString(),
                Exchange = execution.Exchange,
                ExecutionTime = execution.ExecutionTime,
                Id = execution.Id,
                OrderId = execution.OrderId,
                OrderOrigin = execution.OrderOrigin,
                PermanentID = execution.PermanentID,
                Price = execution.Price,
                Quantity = execution.Quantity,
                RealizedPnL = execution.RealizedPnL,
                RealizedPnlPips = execution.RealizedPnlPips,
                RealizedPnlUsd = execution.RealizedPnlUsd,
                Side = execution.Side,
                Strategy = execution.Strategy,
                TradeDuration = execution.TradeDuration
            };
        }
    }
}
