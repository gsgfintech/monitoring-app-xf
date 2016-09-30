using Capital.GSG.FX.Monitoring.AppDataTypes;

namespace MonitoringApp.XF.Components.Executions
{
    public class ExecutionFullViewModel : ExecutionFull
    {
        public string Header { get { return $"{Side} {Quantity / 1000:N0}K {Cross} @ {Price}"; } }

        public string FormattedCommission { get { return $"{Commission:N2} {CommissionCcy}"; } }
    }

    public static class ExecutionFullViewModelExtensions
    {
        public static ExecutionFullViewModel ToExecutionFullViewModel(this ExecutionFull execution)
        {
            if (execution == null)
                return null;

            return new ExecutionFullViewModel()
            {
                AccountNumber = execution.AccountNumber,
                ClientId = execution.ClientId,
                ClientOrderRef = execution.ClientOrderRef,
                Commission = execution.Commission,
                CommissionCcy = execution.CommissionCcy,
                CommissionUsd = execution.CommissionUsd,
                Cross = execution.Cross,
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
