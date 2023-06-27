using Microsoft.Datasync.Client.SQLiteStore;
using Microsoft.Datasync.Client;
using Monoregion.App.Entites;
using System;
using System.Threading.Tasks;

namespace Monoregion.App.Helpers
{
    public static class DatasyncClientHelper
    {
        public static DatasyncClient GetDatasyncClient()
        {
            var options = new DatasyncClientOptions()
            {
                OfflineStore = GetOfflineStore(),
            };

            var client = new DatasyncClient(Configuration.Instance.ServiceUri, options);

            // TODO: add task source complete binding
            Task.Run(async () =>
            {
                await client.InitializeOfflineStoreAsync();
            }).Wait();

            return client;
        }

        private static OfflineSQLiteStore GetOfflineStore()
        {
            var store = new OfflineSQLiteStore(GetPathForLocalDB());

            store.DefineTable<DirectionModel>();
            store.DefineTable<RecordModel>();
            store.DefineTable<GlobalEnvironmentVariable>();

            return store;
        }

        private static string GetPathForLocalDB()
        {
            return new UriBuilder
            {
                Scheme = "file",
                Path = $"{Xamarin.Essentials.FileSystem.AppDataDirectory}/{Configuration.Instance.DbSourceFileName}",
                Query = "?mode=rwc",
            }.Uri.ToString();
        }
    }
}
