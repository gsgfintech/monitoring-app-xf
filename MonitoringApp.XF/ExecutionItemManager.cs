using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capital.GSG.FX.Monitor.App.XF
{
    public partial class ExecutionItemManager
    {
        private static ExecutionItemManager defaultInstance = new ExecutionItemManager();

        private MobileServiceClient client;

#if OFFLINE_SYNC_ENABLED
        IMobileServiceSyncTable<ExecutionItem> executionsTable;
#else
        IMobileServiceTable<ExecutionItem> executionsTable;
#endif

        private ExecutionItemManager()
        {
            //this.client = new MobileServiceClient(Constants.ApplicationURL);
            client = App.Client;

#if OFFLINE_SYNC_ENABLED
            var store = new MobileServiceSQLiteStore("localstore.db");
            store.DefineTable<ExecutionItem>();

            //Initializes the SyncContext using the default IMobileServiceSyncHandler.
            this.client.SyncContext.InitializeAsync(store);

            this.executionsTable = client.GetSyncTable<ExecutionItem>();
#else
            this.executionsTable = client.GetTable<ExecutionItem>();
#endif
        }

        public static ExecutionItemManager DefaultManager
        {
            get { return defaultInstance; }
            private set { defaultInstance = value; }
        }

        public MobileServiceClient CurrentClient { get { return client; } }

        public bool IsOfflineEnabled
        {
            get { return executionsTable is Microsoft.WindowsAzure.MobileServices.Sync.IMobileServiceSyncTable<ExecutionItem>; }
        }

        public async Task<ObservableCollection<ExecutionItem>> GetExecutionItemsAsync(bool syncItems = false)
        {
            try
            {
#if OFFLINE_SYNC_ENABLED
                if (syncItems)
                {
                    await this.SyncAsync();
                }
#endif
                IEnumerable<ExecutionItem> items = await executionsTable.ToEnumerableAsync();

                return new ObservableCollection<ExecutionItem>(items);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine($"Invalid sync operation: {msioe.Message}");
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Sync error: {e.Message}");
            }

            return null;
        }

#if OFFLINE_SYNC_ENABLED
        public async Task SyncAsync()
        {
            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

            try
            {
                await this.client.SyncContext.PushAsync();

                await this.executionsTable.PullAsync(
                    //The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                    //Use a different query name for each unique query in your program
                    "allExecutionItems",
                    this.executionsTable.CreateQuery());
            }
            catch (MobileServicePushFailedException exc)
            {
                if (exc.PushResult != null)
                {
                    syncErrors = exc.PushResult.Errors;
                }
            }

            // Simple error/conflict handling. A real application would handle the various errors like network conditions,
            // server conflicts and others via the IMobileServiceSyncHandler.
            if (syncErrors != null)
            {
                foreach (var error in syncErrors)
                {
                    if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
                    {
                        //Update failed, reverting to server's copy.
                        await error.CancelAndUpdateItemAsync(error.Result);
                    }
                    else
                    {
                        // Discard local change.
                        await error.CancelAndDiscardItemAsync();
                    }

                    Debug.WriteLine($"Error executing sync operation. Item: {error.TableName} ({error.Item["id"]}). Operation discarded.");
                }
            }
        }
#endif
    }
}
