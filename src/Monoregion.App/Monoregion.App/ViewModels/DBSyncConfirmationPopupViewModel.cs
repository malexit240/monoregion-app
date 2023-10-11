using System.Threading.Tasks;
using System.Windows.Input;
using Monoregion.App.Helpers;
using Prism.Navigation;

namespace Monoregion.App.ViewModels
{
    public class DBSyncConfirmationPopupViewModel : BaseViewModel
    {
        public DBSyncConfirmationPopupViewModel(
            INavigationService navigationService)
            : base(navigationService)
        {
        }

        private string _serviceUri;
        public string ServiceUri
        {
            get => _serviceUri;
            set => SetProperty(ref _serviceUri, value);
        }

        private ICommand _OkTappedCommand;
        public ICommand OkTappedCommand => SingleExecutionCommand.FromFunc(OnOkTappedCommandAsync);

        private ICommand _CancelTappedCommand;
        public ICommand CancelTappedCommand => SingleExecutionCommand.FromFunc(OnCancelTappedCommandAsync);

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            ServiceUri = Configuration.Instance.ServiceUri;
        }

        private async Task OnOkTappedCommandAsync()
        {
            Configuration.Instance.ServiceUri = ServiceUri;

            await NavigationService.GoBackAsync(
                new NavigationParameters()
                {
                    {  Constants.Navigation.SYNC_DATABASE_CONFIRMED, true },
                });
        }

        private async Task OnCancelTappedCommandAsync()
        {
            await NavigationService.GoBackAsync();
        }
    }
}
