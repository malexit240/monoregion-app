using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Navigation;
using Xamarin.Forms;
using Monoregion.App.Services.DirectionService;
using Monoregion.App.Services.SystemsService;
using Monoregion.App.Entites;
using Monoregion.App.Helpers;
using Monoregion.App.Extensions;
using Monoregion.App.Views;

namespace Monoregion.App.ViewModels
{
    public class DirectionsPageViewModel : BaseViewModel
    {
        private readonly IDirectionService _directionService;
        private readonly ISystemsService _systemsService;

        public DirectionsPageViewModel(
            INavigationService navigationService,
            IDirectionService directionService,
            ISystemsService systemsService)
            : base(navigationService)
        {
            _directionService = directionService;
            _systemsService = systemsService;
        }

        #region -- Public Properties --

        private ObservableCollection<DirectionViewModel> _directions;
        public ObservableCollection<DirectionViewModel> Directions
        {
            get => _directions;
            set => SetProperty(ref _directions, value);
        }

        private int _dbVersion;
        public int DbVersion
        {
            get => _dbVersion;
            set => SetProperty(ref _dbVersion, value);
        }

        public ICommand AddDirectionTappedCommand => SingleExecutionCommand.FromFunc(OnAddDirectionTappedCommandAsync);

        public ICommand DirectionTappedCommand => SingleExecutionCommand.FromFunc<DirectionViewModel>(OnDirectionTappedCommandAsync);

        public ICommand DirectionDeleteTappedCommand => SingleExecutionCommand.FromFunc<DirectionViewModel>(OnDirectionDeleteTappedCommandAsync);

        public ICommand MakeBackUpCommand => SingleExecutionCommand.FromFunc(OnMakeBackUpCommandAsync);

        public ICommand RestoreDBCommand => SingleExecutionCommand.FromFunc(OnRestoreDBCommandAsync);

        private Task OnRestoreDBCommandAsync()
        {
            return NavigationService.NavigateAsync(nameof(RestoreDBAlertPopupPage), useModalNavigation: true);
        }

        private Task OnMakeBackUpCommandAsync()
        {
            using (var context = new DatabaseContext())
            {
                return context.BackupDBAsync();
            }
        }

        #endregion

        #region -- Overrides --

        private bool _wasInitializeCalled = false;
        public override async void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            _wasInitializeCalled = true;

            await LoadDirectionsAsync();
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (!_wasInitializeCalled)
            {
                await LoadDirectionsAsync();
            }

            await LoadDBVersionAsync();

            _wasInitializeCalled = false;
        }

        #endregion

        #region -- Private Helpers --

        private async Task OnDirectionDeleteTappedCommandAsync(DirectionViewModel direction)
        {
            await _directionService.DeleteDirectionAsync(direction.ToModel());

            await LoadDirectionsAsync();
        }

        private async Task OnAddDirectionTappedCommandAsync()
        {
            await NavigationService.NavigateAsync(nameof(AddDirectionPopupPage), useModalNavigation: true);
        }

        private async Task OnDirectionTappedCommandAsync(DirectionViewModel direction)
        {
            await NavigationService.NavigateAsync(nameof(RecordsPage), new NavigationParameters() { { Constants.Navigation.DIRECTION_VIEW_MODEL, direction.ToModel() } });
        }

        private async Task LoadDirectionsAsync()
        {
            var directions = await _directionService.GetAllDirectionsAsync();

            Directions = new ObservableCollection<DirectionViewModel>(directions.Select(d => d.ToViewModel(DirectionTappedCommand, DirectionDeleteTappedCommand)));
        }

        private async Task LoadDBVersionAsync()
        {
            DbVersion = int.Parse(await _systemsService.GetAsync("DB_VERSION", "0"));
        }

        #endregion
    }
}
