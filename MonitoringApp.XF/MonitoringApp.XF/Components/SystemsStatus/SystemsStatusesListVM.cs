using Capital.GSG.FX.Monitoring.AppDataTypes;
using Capital.GSG.FX.Utils.Portable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MonitoringApp.XF.Components.SystemsStatus
{
    public class SystemsStatusesListVM
    {
        public async Task<List<Tuple<string, string, Color>>> RefreshSystemsStatuses(bool refresh)
        {
            try
            {
                return await LoadSystemsStatuses(refresh);
            }
            catch (AuthenticationRequiredException)
            {
                if (App.Authenticator != null)
                {
                    bool authenticated = await App.Authenticator.AuthenticateAsync();

                    if (authenticated)
                        await LoadSystemsStatuses(refresh);
                }

                return null;
            }
        }

        private async Task<List<Tuple<string, string, Color>>> LoadSystemsStatuses(bool refresh)
        {
            var statuses = await SystemStatusManager.Instance.LoadSystemsStatuses(refresh);

            if (statuses.IsNullOrEmpty())
                return null;
            else
                return statuses.Select(e =>
                {
                    string status = e.IsAlive ? "Running" : "Stopped";

                    Color color;
                    switch (e.OverallStatus)
                    {
                        case "GREEN":
                            color = Color.Green;
                            break;
                        case "YELLOW":
                            color = Color.Yellow;
                            break;
                        default:
                            color = Color.Red;
                            break;
                    }

                    return new Tuple<string, string, Color>(e.Name, status, color);
                }).ToList();
        }

        public async Task<SystemStatusFull> GetSystemStatusByName(string name)
        {
            try
            {
                return await LoadSystemStatusByName(name);
            }
            catch (AuthenticationRequiredException)
            {
                if (App.Authenticator != null)
                {
                    bool authenticated = await App.Authenticator.AuthenticateAsync();

                    if (authenticated)
                        await LoadSystemStatusByName(name);
                }

                return null;
            }
        }

        private static async Task<SystemStatusFull> LoadSystemStatusByName(string name)
        {
            return await SystemStatusManager.Instance.GetSystemStatusByName(name);
        }
    }
}
