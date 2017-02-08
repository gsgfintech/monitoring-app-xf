using Capital.GSG.FX.Data.Core.AccountPortfolio;
using Capital.GSG.FX.Monitoring.Server.Connector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace MonitoringApp.XF.Managers
{
    public class PnLManager
    {
        private const string PnLRoute = "pnl";

        private readonly BackendPnLsConnector pnlsConnector;

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
            pnlsConnector = App.MonitoringServerConnector.PnLsConnector;
        }

        public async Task<PnL> LoadPnL(DateTime day, bool refresh)
        {
            try
            {
                if (!pnlDict.ContainsKey(day) || pnlDict[day] == null || refresh)
                {
                    CancellationTokenSource cts = new CancellationTokenSource();
                    cts.CancelAfter(TimeSpan.FromMinutes(1));

                    var pnl = await pnlsConnector.GetForDay(day, cts.Token);

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
            catch (Exception e)
            {
                Debug.WriteLine($"Sync error: {e.Message}");
                return null;
            }
        }
    }
}
