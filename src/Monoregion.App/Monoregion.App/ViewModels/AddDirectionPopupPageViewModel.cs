using System.Threading.Tasks;
using System.Windows.Input;
using Monoregion.App.Entites;
using Monoregion.App.Helpers;
using Monoregion.App.Services.DirectionService;
using Prism.Navigation;

namespace Monoregion.App.ViewModels
{
    public class AddDirectionPopupPageViewModel : BaseViewModel
    {
        private readonly IDirectionService _directionService;

        public AddDirectionPopupPageViewModel(
            INavigationService navigationService,
            IDirectionService directionService)
            : base(navigationService)
        {
            _directionService = directionService;
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private ICommand _OkTappedCommand;
        public ICommand OkTappedCommand => SingleExecutionCommand.FromFunc(OnOkTappedCommandAsync);

        private ICommand _CancelTappedCommand;
        public ICommand CancelTappedCommand => SingleExecutionCommand.FromFunc(OnCancelTappedCommandAsync);

        private async Task OnOkTappedCommandAsync()
        {
            if (!string.IsNullOrEmpty(Name))
            {
                await _directionService.AddDirectionAsync(new DirectionModel()
                {
                    Name = Name,
                });

                await NavigationService.GoBackAsync();
            }
        }

        private async Task OnCancelTappedCommandAsync()
        {
            await NavigationService.GoBackAsync();
        }
    }

    public class RestoreDBAlertPopupPageViewModel : BaseViewModel
    {
        public RestoreDBAlertPopupPageViewModel(
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
            using (var context = new DatabaseContext())
            {
                await context.RestoreDBAsync();
            }

            await NavigationService.GoBackAsync();
        }

        private async Task OnCancelTappedCommandAsync()
        {
            await NavigationService.GoBackAsync();
        }
    }
}
