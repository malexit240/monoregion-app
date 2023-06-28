using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Navigation;
using Microsoft.Datasync.Client;
using Monoregion.App.Services.DirectionService;
using Monoregion.App.Entites;
using Monoregion.App.Helpers;
using Monoregion.App.Extensions;
using Monoregion.App.Views;

namespace Monoregion.App.ViewModels
{
    public class DirectionsPageViewModel : BaseViewModel
    {
        private readonly IDirectionService _directionService;
        private readonly DatasyncClient _client;

        public DirectionsPageViewModel(
            INavigationService navigationService,
            IDirectionService directionService,
            DatasyncClient client)
            : base(navigationService)
        {
            _directionService = directionService;
            _client = client;
        }

        #region -- Public Properties --

        private ObservableCollection<DirectionViewModel> _directions;
        public ObservableCollection<DirectionViewModel> Directions
        {
            get => _directions;
            set => SetProperty(ref _directions, value);
        }

        public ICommand AddDirectionTappedCommand => SingleExecutionCommand.FromFunc(OnAddDirectionTappedCommandAsync);

        public ICommand DirectionTappedCommand => SingleExecutionCommand.FromFunc<DirectionViewModel>(OnDirectionTappedCommandAsync);

        public ICommand DirectionDeleteTappedCommand => SingleExecutionCommand.FromFunc<DirectionViewModel>(OnDirectionDeleteTappedCommandAsync);

        public ICommand MakeBackUpCommand => SingleExecutionCommand.FromFunc(OnRefreshDbCommandAsync);

        public ICommand RestoreDBCommand => SingleExecutionCommand.FromFunc(OnRefreshDbCommandAsync);

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

            if (parameters.ContainsKey(Constants.Navigation.SYNC_DATABASE_CONFIRMED))
            {
                await RefreshDbCommandAsync();
            }
            else if (!_wasInitializeCalled)
            {
                await LoadDirectionsAsync();
            }

            _wasInitializeCalled = false;
        }

        #endregion

        #region -- Private Helpers --

        private Task OnRefreshDbCommandAsync()
        {
            return NavigationService.NavigateAsync(nameof(DBSyncConfirmationPopupPage), useModalNavigation: true);
        }


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

        private async Task RefreshDbCommandAsync()
        {
            try
            {
                await _client.PushTablesAsync();
                await _client.PullTablesAsync();

                await LoadDirectionsAsync();
            }
            catch (System.Exception ex)
            {
                // It is ordinary case if exception raised here
                System.Diagnostics.Debug.WriteLine("Error at datasync attemption");
            }
        }

        private async Task LoadDirectionsAsync()
        {
            var directions = await _directionService.GetAllDirectionsAsync();

            Directions = new ObservableCollection<DirectionViewModel>(directions.Select(d => d.ToViewModel(DirectionTappedCommand, DirectionDeleteTappedCommand)));
        }

        #endregion
    }
}
