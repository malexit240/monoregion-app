using Microsoft.Datasync.Client;
using Monoregion.App.Entites;
using System;
using System.Threading.Tasks;

namespace Monoregion.App.Helpers
{
    public static class DatasyncOperationsHelper
    {
        private static TaskCompletionSource<bool> _isInitializationCompleted = new TaskCompletionSource<bool>();

        public static async Task<G> PerformOfflineTableOperation<T, G>(this DatasyncClient client, Func<IOfflineTable<T>, Task<G>> operationToPerform)
        {
            if (await _isInitializationCompleted.Task)
            {
                var table = client.GetOfflineTable<T>();

                G result = await operationToPerform(table);

                return result;
            }
            else
            {
                throw new DatasyncInvalidOperationException("Datasync not initializaed");
            }
        }

        public static async Task PerformOfflineTableOperation<T>(this DatasyncClient client, Func<IOfflineTable<T>, Task> operationToPerform)
        {
            if (await _isInitializationCompleted.Task)
            {

                await _isInitializationCompleted.Task;

                var table = client.GetOfflineTable<T>();

                await operationToPerform(table);
            }
            else
            {
                throw new DatasyncInvalidOperationException("Datasync not initializaed");
            }
        }

        public static async Task PullTablesAsync(this DatasyncClient client)
        {
            if (await _isInitializationCompleted.Task)
            {
                await _isInitializationCompleted.Task;

                await client.GetOfflineTable<DirectionModel>().PullItemsAsync();
                await client.GetOfflineTable<RecordModel>().PullItemsAsync();
            }
            else
            {
                throw new DatasyncInvalidOperationException("Datasync not initializaed");
            }
        }

        public static void SetWetherDatasyncInitializationCompleted(bool state)
        {
            _isInitializationCompleted.TrySetResult(state);
        }
    }
}
