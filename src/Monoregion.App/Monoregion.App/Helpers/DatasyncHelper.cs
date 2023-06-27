using Microsoft.Datasync.Client;
using Monoregion.App.Entites;
using System;
using System.Threading.Tasks;

namespace Monoregion.App.Helpers
{
    public static class DatasyncHelper
    {
        public static async Task<G> PerformOfflineTableOperation<T, G>(this DatasyncClient client, Func<IOfflineTable<T>, Task<G>> operationToPerform)
        {
            var table = client.GetOfflineTable<T>();

            G result = await operationToPerform(table);

            return result;
        }

        public static async Task PullTablesAsync(this DatasyncClient client)
        {
            await client.GetOfflineTable<DirectionModel>().PullItemsAsync();
            await client.GetOfflineTable<RecordModel>().PullItemsAsync();
            //await client.GetOfflineTable<GlobalEnvironmentVariable>().PullItemsAsync();
        }

        public static async Task PerformOfflineTableOperation<T>(this DatasyncClient client, Func<IOfflineTable<T>, Task> operationToPerform)
        {
            var table = client.GetOfflineTable<T>();

            await operationToPerform(table);
        }
    }
}
