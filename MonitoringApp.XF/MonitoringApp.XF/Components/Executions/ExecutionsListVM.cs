using Capital.GSG.FX.Monitoring.AppDataTypes;
using Capital.GSG.FX.Utils.Portable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonitoringApp.XF.Components.Executions
{
    public class ExecutionsListVM
    {
        public async Task<List<Tuple<string, string, string>>> RefreshTodaysExecutions(bool refresh)
        {
            var executions = await ExecutionManager.Instance.LoadTodaysExecutions(refresh);

            if (executions.IsNullOrEmpty())
                return null;
            else
                return executions.Select(e =>
                {
                    string title = $"{e.Side} {e.Quantity / 1000}K {e.Cross} @ {e.Price}";

                    StringBuilder details = new StringBuilder($"{e.ExecutionTime:dd/MM/yy HH:mm:ss} | {ShortenOrderOrigin(e.OrderOrigin)}");

                    if (e.RealizedPnlUsd.HasValue)
                    {
                        string prefix = e.RealizedPnlUsd.Value >= 0 ? "+" : "";
                        details.Append($" | {prefix}{e.RealizedPnlUsd:N0} USD");
                    }

                    if (!string.IsNullOrEmpty(e.TradeDuration))
                        details.Append($" | {e.TradeDuration}");

                    return new Tuple<string, string, string>(e.Id, title, details.ToString());
                }).ToList();
        }

        public async Task<ExecutionFull> GetExecutionById(string id)
        {
            return await ExecutionManager.Instance.GetExecutionById(id);
        }

        private string ShortenOrderOrigin(string orderOrigin)
        {
            switch (orderOrigin)
            {
                case "PositionOpen":
                    return "PO";
                case "PositionClose":
                    return "PC";
                case "PositionClose_ContStop":
                    return "PCS";
                case "PositionClose_ContLimit":
                    return "PCL";
                case "PositionClose_TE":
                    return "PCT";
                case "PositionClose_CircuitBreaker":
                    return "PCB";
                case "PositionReverse_Close":
                    return "PRC";
                case "PositionReverse_Open":
                    return "PRO";
                default:
                    return orderOrigin;
            }
        }
    }
}
