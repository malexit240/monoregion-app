using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Monoregion.App.Entites;
using Monoregion.App.Extensions;
using Monoregion.App.Helpers;
using Monoregion.App.Services.RecordService;
using Prism.Navigation;
using Xamarin.Forms;

namespace Monoregion.App.ViewModels
{
    public class AddEditRecordPageViewModel : BaseViewModel
    {
        private readonly IRecordsService _recordsService;

        private DirectionModel _direction;
        private bool _isEditMode;
        private bool _isTimerAlive = true;

        public AddEditRecordPageViewModel(
            INavigationService navigationService,
            IRecordsService recordsService)
            : base(navigationService)
        {
            _recordsService = recordsService;

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
                _isEditMode = true;
                Record = record.ToViewModel(null, null);
            }

            if (parameters.TryGetValue<DirectionModel>(Constants.Navigation.DIRECTION_VIEW_MODEL, out var direction))
            {
                _direction = direction;

                if (!_isEditMode)
                {
                    Record = new RecordViewModel(null, null)
                    {
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
            bool hasOwnName = !string.IsNullOrWhiteSpace(Record.Name);
            if (!hasOwnName)
            {
                Record.Name = Record.Id.ToString().Substring(0, 4);
            }

            if (_isEditMode)
            {
                await _recordsService.UpdateRecordAsync(Record.ToModel());
            }
            else
            {
                Record.CreationTime = DateTime.Now;
                _isEditMode = true;
                await _recordsService.AddRecordAsync(Record.ToModel());
            }
        }
    }
}
