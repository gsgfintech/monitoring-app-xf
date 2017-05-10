using Capital.GSG.FX.Data.Core.AccountPortfolio;
using Capital.GSG.FX.Data.Core.ContractData;
using Capital.GSG.FX.Monitoring.Server.Connector;
using Capital.GSG.FX.Utils.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MonitoringApp.XF.Components.Positions
{
    public class PositionManager
    {
        private const string AccountsRoute = "accounts";
        private const string PositionsRoute = "positions";

        private readonly BackendAccountsConnector accountsConnector;
        private readonly BackendPositionsConnector positionsConnector;

        private static PositionManager instance;
        public static PositionManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new PositionManager();

                return instance;
            }
        }

        private Dictionary<Tuple<Broker, string>, Account> accountsDict;
        private Dictionary<(Broker Broker, string Account, Cross Cross), Position> positionsDict;

        private PositionManager()
        {
            accountsConnector = App.MonitoringServerConnector.AccountsConnector;
            positionsConnector = App.MonitoringServerConnector.PositionsConnector;
        }

        public async Task<List<Position>> LoadPositions(bool refresh)
        {
            try
            {
                if (positionsDict == null || refresh)
                {
                    CancellationTokenSource cts = new CancellationTokenSource();
                    cts.CancelAfter(TimeSpan.FromMinutes(1));

                    var positionsResult = await positionsConnector.GetAll(cts.Token);

                    if (positionsResult.Success && !positionsResult.Positions.IsNullOrEmpty())
                        positionsDict = positionsResult.Positions.ToDictionary(p => (p.Broker, p.Account, p.Cross), p => p);
                    else
                        positionsDict = new Dictionary<(Broker Broker, string Account, Cross Cross), Position>();
                }

                return positionsDict.Values.ToList();
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Not retrieving positions: operation cancelled");
                return null;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Sync error: {e.Message}");
                return null;
            }
        }

        public async Task<List<Account>> LoadAccounts(bool refresh)
        {
            try
            {
                if (accountsDict == null || refresh)
                {
                    CancellationTokenSource cts = new CancellationTokenSource();
                    cts.CancelAfter(TimeSpan.FromMinutes(1));

                    var result = await accountsConnector.GetAll(cts.Token);

                    if (result.Success && !result.Accounts.IsNullOrEmpty())
                        accountsDict = result.Accounts.ToDictionary(a => new Tuple<Broker, string>(a.Broker, a.Name), a => a);
                    else
                        accountsDict = new Dictionary<Tuple<Broker, string>, Account>();
                }

                return accountsDict.Values.ToList();
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Not retrieving accounts: operation cancelled");
                return null;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Sync error: {e.Message}");
                return null;
            }
        }

        public async Task<Position> GetPositionByCross(Broker broker, string account, Cross cross)
        {
            try
            {
                if (positionsDict.ContainsKey((broker, account, cross)) && positionsDict[(broker, account, cross)] != null)
                    return positionsDict[(broker, account, cross)];
                else
                {
                    CancellationTokenSource cts = new CancellationTokenSource();
                    cts.CancelAfter(TimeSpan.FromMinutes(1));

                    var result = await positionsConnector.Get(broker, account, cross, cts.Token);

                    if (result.Success && result.Position != null)
                        positionsDict[(broker, account, cross)] = result.Position;

                    return result.Position;
                }
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Not retrieving position's details: operation cancelled");
                return null;
            }
            catch (ArgumentNullException ex)
            {
                Debug.WriteLine($"Not retrieving position's details: missing or invalid parameter {ex.ParamName}");
                return null;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Sync error: {e.Message}");
                return null;
            }
        }

        public async Task<Account> GetAccount(Broker broker, string accountName)
        {
            try
            {
                Tuple<Broker, string> key = new Tuple<Broker, string>(broker, accountName);

                if (accountsDict.ContainsKey(key) && accountsDict[key] != null)
                    return accountsDict[key];
                else
                {
                    CancellationTokenSource cts = new CancellationTokenSource();
                    cts.CancelAfter(TimeSpan.FromMinutes(1));

                    var result = await accountsConnector.Get(broker, accountName, cts.Token);

                    if (result.Success && result.Account != null)
                        accountsDict[key] = result.Account;

                    return result.Account;
                }
            }
            catch (OperationCanceledException)
            {
                Debug.WriteLine("Not retrieving account's details: operation cancelled");
                return null;
            }
            catch (ArgumentNullException ex)
            {
                Debug.WriteLine($"Not retrieving account's details: missing or invalid parameter {ex.ParamName}");
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
