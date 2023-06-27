using Xamarin.Forms;
using Prism.Ioc;
using Monoregion.App.Views;
using Monoregion.App.ViewModels;
using Monoregion.App.Services.EnvironmentVariableService;
using Monoregion.App.Services.DirectionService;
using Monoregion.App.Services.RecordService;
using Microsoft.Datasync.Client;
using Monoregion.App.Entites;
using Microsoft.Datasync.Client.SQLiteStore;
using Xamarin.Essentials;
using System.Threading.Tasks;
using System;

namespace Monoregion.App
{
    public partial class App
    {
        public App()
        {
            InitializeComponent();
        }

        protected override async void OnInitialized()
        {
            await NavigationService.NavigateAsync($"/{nameof(NavigationPage)}/{nameof(DirectionsPage)}");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<DirectionsPage, DirectionsPageViewModel>();
            containerRegistry.RegisterForNavigation<RecordsPage, RecordsPageViewModel>();
            containerRegistry.RegisterForNavigation<AddEditRecordPage, AddEditRecordPageViewModel>();
            containerRegistry.RegisterForNavigation<AddDirectionPopupPage, AddDirectionPopupPageViewModel>();
            containerRegistry.RegisterForNavigation<RestoreDBAlertPopupPage, RestoreDBAlertPopupPageViewModel>();

            containerRegistry.RegisterInstance<DatasyncClient>(GetDatasyncClient());
            containerRegistry.RegisterInstance<IEnvironmentVariableService>(Container.Resolve<EnvironmentVariablesService>());
            containerRegistry.RegisterInstance<IDirectionService>(Container.Resolve<RestDirectionService>());
            containerRegistry.RegisterInstance<IRecordsService>(Container.Resolve<RestRecordService>());
        }

        private DatasyncClient GetDatasyncClient()
        {

            var p = Configuration.Instance.DbSourceFileName;
            var path = new UriBuilder { Scheme = "file", Path = $"{Xamarin.Essentials.FileSystem.AppDataDirectory}/{Configuration.Instance.DbSourceFileName}", Query = "?mode=rwc" }.Uri.ToString();

            var store = new OfflineSQLiteStore(path);

            store.DefineTable<DirectionModel>();
            store.DefineTable<RecordModel>();
            store.DefineTable<GlobalEnvironmentVariable>();

            var options = new DatasyncClientOptions()
            {
                OfflineStore = store,
            };

            var client = new DatasyncClient(Configuration.Instance.ServiceUri, options);

            // TODO: add task source complete binding

            Task.Run(async () =>
            {
                await client.InitializeOfflineStoreAsync();
            }).Wait();
            return client;
        }
    }
}
