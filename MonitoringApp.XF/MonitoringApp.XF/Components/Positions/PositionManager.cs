﻿using Capital.GSG.FX.Data.Core.AccountPortfolio;
using Capital.GSG.FX.Data.Core.ContractData;
using Capital.GSG.FX.Utils.Portable;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MonitoringApp.XF.Components.Positions
{
    public class PositionManager
    {
        private const string AccountsRoute = "accounts";
        private const string PositionsRoute = "positions";

        private readonly MobileServiceClient client;

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
            client = App.MobileServiceClient;
        }

        public async Task<List<Position>> LoadPositions(bool refresh)
        {
            try
            {
                if (positionsDict == null || refresh)
                {
                    CancellationTokenSource cts = new CancellationTokenSource();
                    cts.CancelAfter(TimeSpan.FromMinutes(1));

                    var positions = await client.InvokeApiAsync<List<Position>>(PositionsRoute, HttpMethod.Get, null, cts.Token);

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
            catch (MobileServiceInvalidOperationException msioe)
            {
                if (msioe.Response?.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Debug.WriteLine("Authentication necessary to load positions");
                    throw new AuthenticationRequiredException(typeof(PositionManager)); // Re-throw the unauthorized exception and catch it in the VM to redirect to the login page
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

        public async Task<List<Account>> LoadAccounts(bool refresh)
        {
            try
            {
                if (accountsDict == null || refresh)
                {
                    CancellationTokenSource cts = new CancellationTokenSource();
                    cts.CancelAfter(TimeSpan.FromMinutes(1));

                    var accounts = await client.InvokeApiAsync<List<Account>>(AccountsRoute, HttpMethod.Get, null, cts.Token);

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
            catch (MobileServiceInvalidOperationException msioe)
            {
                if (msioe.Response?.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Debug.WriteLine("Authentication necessary to load accounts");
                    throw new AuthenticationRequiredException(typeof(PositionManager)); // Re-throw the unauthorized exception and catch it in the VM to redirect to the login page
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

                    Position position = await client.InvokeApiAsync<Position>($"{PositionsRoute}/{(int)Broker.IB}/{(int)cross}", HttpMethod.Get, null, cts.Token);

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
            catch (MobileServiceInvalidOperationException msioe)
            {
                if (msioe.Response?.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Debug.WriteLine("Authentication necessary to load position");
                    throw new AuthenticationRequiredException(typeof(PositionManager)); // Re-throw the unauthorized exception and catch it in the VM to redirect to the login page
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

                    Account account = await client.InvokeApiAsync<Account>($"{AccountsRoute}/{(int)broker}/{accountName}", HttpMethod.Get, null, cts.Token);

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
            catch (MobileServiceInvalidOperationException msioe)
            {
                if (msioe.Response?.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Debug.WriteLine("Authentication necessary to load account");
                    throw new AuthenticationRequiredException(typeof(PositionManager)); // Re-throw the unauthorized exception and catch it in the VM to redirect to the login page
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
