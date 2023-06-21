using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Monoregion.App.Entites;
using Monoregion.App.Extensions;
using Monoregion.App.Helpers;
using Monoregion.App.Services.RecordService;
using Monoregion.App.Services.SystemsService;
using Prism.Navigation;
using Xamarin.Forms;

namespace Monoregion.App.ViewModels
{
    public class AddEditRecordPageViewModel : BaseViewModel
    {
        private readonly IRecordsService _recordsService;
        private readonly ISystemsService _systemsService;

        private DirectionModel _direction;
        private bool _isEditingMode;
        private bool _isTimerAlive = true;

        public AddEditRecordPageViewModel(
            INavigationService navigationService,
            IRecordsService recordsService,
            ISystemsService systemsService)
            : base(navigationService)
        {
            _recordsService = recordsService;
            _systemsService = systemsService;

            Device.StartTimer(TimeSpan.FromSeconds(45), () =>
            {
                if (_isTimerAlive)
                {
                    System.Diagnostics.Debug.WriteLine($"Saved {this.Record.Name}");
                    OnSaveTappedCommandAsync();
                }

                return _isTimerAlive;
            });
        }

        private RecordViewModel _record;
        public RecordViewModel Record
        {
            get => _record;
            set => SetProperty(ref _record, value);
        }

        private ICommand _saveTappedCommand;
        public ICommand SaveTappedCommand => SingleExecutionCommand.FromFunc(OnSaveTappedCommandAsync);

        public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);

            if (parameters.TryGetValue<RecordModel>(Constants.Navigation.RECORD_VIEW_MODEL, out var record))
            {
                _isEditingMode = true;
                Record = record.ToViewModel(null, null);
            }

            if (parameters.TryGetValue<DirectionModel>(Constants.Navigation.DIRECTION_VIEW_MODEL, out var direction))
            {
                _direction = direction;

                if (!_isEditingMode)
                {
                    Record = new RecordViewModel(null, null)
                    {
                        Id = Guid.NewGuid(),
                        DirectionModelId = _direction.Id,
                        CreationTime = DateTime.Now,
                    };
                }
            }
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

            _isTimerAlive = false;
        }

        private async Task OnSaveTappedCommandAsync()
        {
            bool hasOwnName = !string.IsNullOrEmpty(Record.Name);
            if (!hasOwnName)
            {
                Record.Name = Record.Id.ToString().Substring(0, 4);
            }

            if (_isEditingMode)
            {
                await _recordsService.UpdateRecordAsync(Record.ToModel());
            }
            else
            {
                Record.CreationTime = DateTime.Now;
                _isEditingMode = true;
                await _recordsService.AddRecordAsync(Record.ToModel());
            }

            if (hasOwnName)
            {
                await _systemsService.CommitNewDBVersion();
            }
        }

    }
}
