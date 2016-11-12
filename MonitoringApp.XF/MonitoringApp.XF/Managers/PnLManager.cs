using Capital.GSG.FX.Data.Core.AccountPortfolio;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MonitoringApp.XF.Managers
{
    public class PnLManager
    {
        private const string PnLRoute = "pnl";

        private readonly MobileServiceClient client;

        private static PnLManager instance;
        public static PnLManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new PnLManager();

                return instance;
            }
        }

        private Dictionary<DateTime, PnL> pnlDict = new Dictionary<DateTime, PnL>();

        private PnLManager()
        {
            client = App.MobileServiceClient;
        }

        public async Task<PnL> LoadPnL(DateTime day, bool refresh)
        {
            try
            {
                if (!pnlDict.ContainsKey(day) || pnlDict[day] == null || refresh)
                {
                    CancellationTokenSource cts = new CancellationTokenSource();
                    cts.CancelAfter(TimeSpan.FromMinutes(1));

                    var pnl = await client.InvokeApiAsync<PnL>(PnLRoute, HttpMethod.Get, new Dictionary<string, string>() {
                        { "day", day.ToString("yyyy-MM-dd") }
                    }, cts.Token);

                    if (pnl != null)
                        pnlDict[day] = pnl;
                    else
                        pnlDict[day] = new PnL();
                }

                return pnlDict[day];
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Not retrieving pnl: operation cancelled");
                return null;
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                if (msioe.Response?.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Debug.WriteLine("Authentication necessary to load pnl");
                    throw new AuthenticationRequiredException(typeof(PnLManager)); // Re-throw the unauthorized exception and catch it in the VM to redirect to the login page
                }

                Debug.WriteLine($"Invalid sync operation: {msioe.Message}");
                return null;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Sync error: {e.Message}");
                return null;
            }
        }
    }
}
