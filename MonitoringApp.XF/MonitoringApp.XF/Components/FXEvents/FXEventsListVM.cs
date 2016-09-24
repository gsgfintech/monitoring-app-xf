using Capital.GSG.FX.Monitoring.AppDataTypes;
using Capital.GSG.FX.Utils.Portable;
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
            try
            {
                return await LoadTodaysHighImpactFXEvents(refresh);
            }
            catch (AuthenticationRequiredException)
            {
                if (App.Authenticator != null)
                {
                    bool authenticated = await App.Authenticator.AuthenticateAsync();

                    if (authenticated)
                        await LoadTodaysHighImpactFXEvents(refresh);
                }

                return null;
            }
        }

        private async Task<List<Tuple<string, string, string>>> LoadTodaysHighImpactFXEvents(bool refresh)
        {
            var fxEvents = await FXEventManager.Instance.LoadTodaysHighImpactFXEvents(refresh);

            return FormatFXEventsList(fxEvents);
        }

        public async Task<List<Tuple<string, string, string>>> RefreshCurrentWeeksFXEvents(bool refresh)
        {
            try
            {
                return await LoadCurrentWeeksFXEvents(refresh);
            }
            catch (AuthenticationRequiredException)
            {
                if (App.Authenticator != null)
                {
                    bool authenticated = await App.Authenticator.AuthenticateAsync();

                    if (authenticated)
                        await LoadCurrentWeeksFXEvents(refresh);
                }

                return null;
            }
        }

        private async Task<List<Tuple<string, string, string>>> LoadCurrentWeeksFXEvents(bool refresh)
        {
            var fxEvents = await FXEventManager.Instance.LoadCurrentWeeksFXEvents(refresh);

            return FormatFXEventsList(fxEvents);
        }

        private List<Tuple<string, string, string>> FormatFXEventsList(List<FXEventSlim> fxEvents)
        {
            if (fxEvents.IsNullOrEmpty())
                return null;
            else
                return fxEvents.Select(e =>
                {
                    string details = $"{e.Level} | {e.Currency} | {e.Timestamp:dd/MM/yy HH:mm zzz}";

                    return new Tuple<string, string, string>(e.Id, e.Title, details);
                }).ToList();
        }

        public async Task<FXEventFull> GetFXEventById(string id)
        {
            try
            {
                return await LoadFXEventById(id);
            }
            catch (AuthenticationRequiredException)
            {
                if (App.Authenticator != null)
                {
                    bool authenticated = await App.Authenticator.AuthenticateAsync();

                    if (authenticated)
                        await LoadFXEventById(id);
                }

                return null;
            }
        }

        private static async Task<FXEventFull> LoadFXEventById(string id)
        {
            return await FXEventManager.Instance.GetFXEventById(id);
        }
    }
}
