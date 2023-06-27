using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Navigation;
using Xamarin.Forms;
using Monoregion.App.Services.DirectionService;
using Monoregion.App.Services.EnvironmentVariableService;
using Monoregion.App.Entites;
using Monoregion.App.Helpers;
using Monoregion.App.Extensions;
using Monoregion.App.Views;
using Microsoft.Datasync.Client;
using Monoregion.App.Helpers;
using System.Net.Http;

namespace Monoregion.App.ViewModels
{
    public class DirectionsPageViewModel : BaseViewModel
    {
        private readonly IDirectionService _directionService;
        private readonly IEnvironmentVariableService _systemsService;
        private readonly DatasyncClient _client;

        public DirectionsPageViewModel(
            INavigationService navigationService,
            IDirectionService directionService,
            IEnvironmentVariableService systemsService,
            DatasyncClient client)
            : base(navigationService)
        {
            _directionService = directionService;
            _systemsService = systemsService;
            _client = client;
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

        private async Task OnRestoreDBCommandAsync()
        {
            await _client.PullTablesAsync();

            await LoadDirectionsAsync();
        }

        private async Task OnMakeBackUpCommandAsync()
        {
            await _client.PushTablesAsync();
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

            //await LoadDBVersionAsync();

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
            //DbVersion = int.Parse(await _systemsService.GetAsync("DB_VERSION", "0"));
        }

        #endregion
    }
}
