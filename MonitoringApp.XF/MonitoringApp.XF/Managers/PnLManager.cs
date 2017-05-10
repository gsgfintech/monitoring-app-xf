using Capital.GSG.FX.Data.Core.AccountPortfolio;
using Capital.GSG.FX.Monitoring.Server.Connector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

                    var result = await pnlsConnector.GetForDay(day, cts.Token);

                    if (result.Success && result.PnLs != null)
                    {
                        // Combine all pnls (temporary solution)
                        var combinedPerCross = result.PnLs.Select(p => p.PerCrossPnLs.AsEnumerable()).Aggregate((cur, next) => cur.Concat(next)).GroupBy(p => p.Cross).Select(g => new PnLPerCross()
                        {
                            Cross = g.Key,
                            GrossRealized = g.Select(p => p.GrossRealized).Sum(),
                            GrossUnrealized = g.Select(p => p.GrossUnrealized).Sum(),
                            PipsUnrealized = g.Select(p => p.PipsUnrealized).Sum(),
                            PositionOpenPrice = g.Select(p => p.PositionOpenPrice)?.FirstOrDefault(),
                            PositionOpenTime = g.Select(p => p.PositionOpenTime)?.FirstOrDefault(),
                            PositionSize = g.Select(p => p.PositionSize).Sum(),
                            TotalFees = g.Select(p => p.TotalFees).Sum(),
                            TradesCount = g.Select(p => p.TradesCount).Sum() / g.Count() // TODO: find a nicer solution
                        }).ToList();

                        pnlDict[day] = new PnL()
                        {
                            Account = result.PnLs.Select(p => p.Account).FirstOrDefault(a => a.Contains("F")) ?? result.PnLs.Select(p => p.Account).First(),
                            Broker = Broker.IB,
                            LastUpdate = result.PnLs.Select(p => p.LastUpdate).Max(),
                            PerCrossPnLs = combinedPerCross
                        };
                    }
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
