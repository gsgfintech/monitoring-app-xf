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
            try
            {
                return await LoadExecutions(refresh);
            }
            catch (AuthenticationRequiredException)
            {
                if (App.Authenticator != null)
                {
                    bool authenticated = await App.Authenticator.AuthenticateAsync();

                    if (authenticated)
                        await LoadExecutions(refresh);
                }

                return null;
            }
        }

        private async Task<List<Tuple<string, string, string>>> LoadExecutions(bool refresh)
        {
            var executions = await ExecutionManager.Instance.LoadTodaysExecutions(refresh);

            if (executions.IsNullOrEmpty())
                return null;
            else
                return executions.Select(e =>
                {
                    string title = $"{e.Side} {e.Quantity / 1000}K {e.Cross} @ {e.Price}";

                    StringBuilder details = new StringBuilder($"{e.ExecutionTime:dd/MM/yy HH:mm:ss} | {Utils.ShortenOrderOrigin(e.OrderOrigin)}");

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
            try
            {
                return await LoadExecutionById(id);
            }
            catch (AuthenticationRequiredException)
            {
                if (App.Authenticator != null)
                {
                    bool authenticated = await App.Authenticator.AuthenticateAsync();

                    if (authenticated)
                        await LoadExecutionById(id);
                }

                return null;
            }
        }

        private static async Task<ExecutionFull> LoadExecutionById(string id)
        {
            return await ExecutionManager.Instance.GetExecutionById(id);
        }
    }
}
