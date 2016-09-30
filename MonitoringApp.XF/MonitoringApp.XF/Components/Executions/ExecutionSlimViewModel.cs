using Capital.GSG.FX.Monitoring.AppDataTypes;
using Capital.GSG.FX.Utils.Portable;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace MonitoringApp.XF.Components.Executions
{
    public class ExecutionSlimViewModel : ExecutionSlim
    {
        public string Header { get { return $"{Side} {Quantity / 1000:N0}K {Cross} @ {Price}"; } }

        public string Details { get; set; }
    }

    public static class ExecutionSlimViewModelExtensions
    {
        public static ExecutionSlimViewModel ToExecutionSlimViewModel(this ExecutionSlim execution)
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

            return new ExecutionSlimViewModel()
            {
                Cross = execution.Cross,
                Details = details.ToString(),
                ExecutionTime = execution.ExecutionTime,
                Id = execution.Id,
                OrderOrigin = execution.OrderOrigin,
                Price = execution.Price,
                Quantity = execution.Quantity,
                RealizedPnlUsd = execution.RealizedPnlUsd,
                Side = execution.Side,
                TradeDuration = execution.TradeDuration
            };
        }

        public static ObservableCollection<ExecutionSlimViewModel> ToExecutionSlimViewModels(this IEnumerable<ExecutionSlim> executions)
        {
            if (executions.IsNullOrEmpty())
                return new ObservableCollection<ExecutionSlimViewModel>();
            else
                return new ObservableCollection<ExecutionSlimViewModel>(executions?.Select(e => e.ToExecutionSlimViewModel()));
        }
    }
}
