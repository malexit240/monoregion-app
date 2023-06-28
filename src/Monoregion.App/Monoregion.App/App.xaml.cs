using Xamarin.Forms;
using Prism.Ioc;
using Microsoft.Datasync.Client;
using Monoregion.App.Views;
using Monoregion.App.ViewModels;
using Monoregion.App.Services.DirectionService;
using Monoregion.App.Services.RecordService;
using Monoregion.App.Helpers;

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
            containerRegistry.RegisterForNavigation<DBSyncConfirmationPopupPage, DBSyncConfirmationPopupViewModel>();

            containerRegistry.RegisterInstance<DatasyncClient>(DatasyncClientHelper.GetDatasyncClient());
            containerRegistry.RegisterInstance<IDirectionService>(Container.Resolve<RestDirectionService>());
            containerRegistry.RegisterInstance<IRecordsService>(Container.Resolve<RestRecordService>());
        }
    }
}
