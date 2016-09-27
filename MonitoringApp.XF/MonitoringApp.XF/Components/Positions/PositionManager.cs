using Capital.GSG.FX.Monitoring.AppDataTypes;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MonitoringApp.XF.Components.Positions
{
    public class PositionManager
    {
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

        private List<PositionSecuritySlim> positions;
        private Dictionary<string, PositionSecurityFull> detailedPositions = new Dictionary<string, PositionSecurityFull>();

        private PositionManager()
        {
            client = App.MobileServiceClient;
        }

        public async Task<List<PositionSecuritySlim>> LoadPositions(bool refresh)
        {
            try
            {
                if (positions == null || refresh)
                {
                    CancellationTokenSource cts = new CancellationTokenSource();
                    cts.CancelAfter(TimeSpan.FromMinutes(1));

                    positions = await client.InvokeApiAsync<List<PositionSecuritySlim>>(PositionsRoute, HttpMethod.Get, null, cts.Token);
                }

                return positions;
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

        public async Task<PositionSecurityFull> GetPositionByCross(string cross)
        {
            try
            {
                if (string.IsNullOrEmpty(cross))
                    throw new ArgumentNullException(nameof(cross));

                if (detailedPositions.ContainsKey(cross) && detailedPositions[cross] != null)
                    return detailedPositions[cross];
                else
                {
                    CancellationTokenSource cts = new CancellationTokenSource();
                    cts.CancelAfter(TimeSpan.FromMinutes(1));

                    PositionSecurityFull position = await client.InvokeApiAsync<PositionSecurityFull>($"{PositionsRoute}/{cross}", HttpMethod.Get, null, cts.Token);

                    if (position != null)
                        detailedPositions[cross] = position;

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
                    Debug.WriteLine("Authentication necessary to load execution");
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
