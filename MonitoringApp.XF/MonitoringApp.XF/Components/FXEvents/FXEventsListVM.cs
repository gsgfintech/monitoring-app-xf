using Capital.GSG.FX.Data.Core.MarketData;
using Capital.GSG.FX.Utils.Core;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonitoringApp.XF.Components.FXEvents
{
    public class FXEventsListVM
    {
        public async Task<List<Tuple<string, string, string>>> RefreshTodaysHighImpactFXEvents(bool refresh)
        {
            return await LoadTodaysHighImpactFXEvents(refresh);
        }

        private async Task<List<Tuple<string, string, string>>> LoadTodaysHighImpactFXEvents(bool refresh)
        {
            var fxEvents = await FXEventManager.Instance.LoadTodaysHighImpactFXEvents(refresh);

            return FormatFXEventsList(fxEvents);
        }

        public async Task<List<Tuple<string, string, string>>> RefreshCurrentWeeksFXEvents(bool refresh)
        {
            return await LoadCurrentWeeksFXEvents(refresh);
        }

        private async Task<List<Tuple<string, string, string>>> LoadCurrentWeeksFXEvents(bool refresh)
        {
            var fxEvents = await FXEventManager.Instance.LoadCurrentWeeksFXEvents(refresh);

            return FormatFXEventsList(fxEvents);
        }

        private List<Tuple<string, string, string>> FormatFXEventsList(List<FXEvent> fxEvents)
        {
            if (fxEvents.IsNullOrEmpty())
                return null;
            else
                return fxEvents.Select(e =>
                {
                    string details = $"{e.Level} | {e.Currency} | {e.Timestamp:dd/MM/yy HH:mm zzz}";

                    return new Tuple<string, string, string>(e.EventId, e.Title, details);
                }).ToList();
        }

        public async Task<FXEvent> GetFXEventById(string id)
        {
            return await LoadFXEventById(id);
        }

        private static async Task<FXEvent> LoadFXEventById(string id)
        {
            return await FXEventManager.Instance.GetFXEventById(id);
        }
    }
}
