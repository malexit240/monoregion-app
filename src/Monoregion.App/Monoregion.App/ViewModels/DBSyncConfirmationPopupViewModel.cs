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

        private ICommand _OkTappedCommand;
        public ICommand OkTappedCommand => SingleExecutionCommand.FromFunc(OnOkTappedCommandAsync);

        private ICommand _CancelTappedCommand;
        public ICommand CancelTappedCommand => SingleExecutionCommand.FromFunc(OnCancelTappedCommandAsync);

        private async Task OnOkTappedCommandAsync()
        {
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
