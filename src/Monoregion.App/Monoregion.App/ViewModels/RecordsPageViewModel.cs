using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Navigation;
using System.Collections.ObjectModel;
using Monoregion.App.Services.DirectionService;
using Monoregion.App.Services.RecordService;
using Monoregion.App.Entites;
using Monoregion.App.Helpers;
using Monoregion.App.Extensions;
using Monoregion.App.Views;

namespace Monoregion.App.ViewModels
{
    public class RecordsPageViewModel : BaseViewModel
    {
        private readonly IDirectionService _directionService;
        private readonly IRecordsService _recordsService;

        public RecordsPageViewModel(
            INavigationService navigationService,
            IDirectionService directionService,
            IRecordsService recordsService)
            : base(navigationService)
        {
            _directionService = directionService;
            _recordsService = recordsService;
        }

        #region -- Public Properties --

        private DirectionViewModel _direction;
        public DirectionViewModel Direction
        {
            get => _direction;
            set => SetProperty(ref _direction, value);
        }

        private ICommand _AddRecordTappedCommand;
        public ICommand AddRecordTappedCommand =>  SingleExecutionCommand.FromFunc(OnAddRecordTappedCommandAsync);

        private ICommand _RecordTappedCommand;
        public ICommand RecordTappedCommand =>  SingleExecutionCommand.FromFunc<RecordViewModel>(OnRecordTappedCommandAsync);

        private ICommand _DeleteRecordTappedCommand;
        public ICommand DeleteRecordTappedCommand => SingleExecutionCommand.FromFunc<RecordViewModel>(OnDeleteRecordTappedCommandAsync);

        #endregion

        private async Task OnDeleteRecordTappedCommandAsync(RecordViewModel record)
        {
            await _recordsService.DeleteRecordAsync(record.ToModel());
            await LoadRecordsAsync();
        }

        private Task OnRecordTappedCommandAsync(RecordViewModel record)
        {
            var parameters = new NavigationParameters()
            {
                { Constants.Navigation.DIRECTION_VIEW_MODEL,Direction.ToModel() },
                { Constants.Navigation.RECORD_VIEW_MODEL, record.ToModel() }
            };

            return NavigationService.NavigateAsync(nameof(AddEditRecordPage), parameters);
        }

        private Task OnAddRecordTappedCommandAsync()
        {
            var parameters = new NavigationParameters()
            {
                { Constants.Navigation.DIRECTION_VIEW_MODEL,Direction.ToModel() }
            };

            return NavigationService.NavigateAsync(nameof(AddEditRecordPage), parameters);
        }

        private bool _wasInitializeCalled = false;
        public override async void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            if (parameters.TryGetValue<DirectionModel>(Constants.Navigation.DIRECTION_VIEW_MODEL, out var direction))
            {
                _wasInitializeCalled = true;
                Direction = direction.ToViewModel(null, null);
                await LoadRecordsAsync();
            }
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (!_wasInitializeCalled)
            {
                await LoadRecordsAsync();
            }

            _wasInitializeCalled = false;
        }

        private async Task LoadRecordsAsync()
        {
            var directions = await _directionService.GetAllDirectionsAsync();

            Direction = directions.FirstOrDefault(d => d.Id == Direction.Id).ToViewModel(null, null);

            Direction.Records = new ObservableCollection<RecordViewModel>(Direction.Records.OrderByDescending(r => r.CreationTime));

            foreach (var record in Direction.Records)
            {
                record.TappedCommand = RecordTappedCommand;
                record.DeleteTappedCommand = DeleteRecordTappedCommand;
            }
        }
    }
}
