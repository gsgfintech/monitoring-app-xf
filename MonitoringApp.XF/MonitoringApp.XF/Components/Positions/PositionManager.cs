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
        private Dictionary<Cross, Position> positionsDict;

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

                    var positions = await positionsConnector.GetAll(cts.Token);

                    if (!positions.IsNullOrEmpty())
                        positionsDict = positions.ToDictionary(p => p.Cross, p => p);
                    else
                        positionsDict = new Dictionary<Cross, Position>();
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

                    var accounts = await accountsConnector.GetAll(cts.Token);

                    if (!accounts.IsNullOrEmpty())
                        accountsDict = accounts.ToDictionary(a => new Tuple<Broker, string>(a.Broker, a.Name), a => a);
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

        public async Task<Position> GetPositionByCross(Cross cross)
        {
            try
            {
                if (positionsDict.ContainsKey(cross) && positionsDict[cross] != null)
                    return positionsDict[cross];
                else
                {
                    CancellationTokenSource cts = new CancellationTokenSource();
                    cts.CancelAfter(TimeSpan.FromMinutes(1));

                    Position position = await positionsConnector.Get(Broker.IB, cross, cts.Token);

                    if (position != null)
                        positionsDict[cross] = position;

                    return position;
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

                    Account account = await accountsConnector.Get(broker, accountName, cts.Token);

                    if (account != null)
                        accountsDict[key] = account;

                    return account;
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
