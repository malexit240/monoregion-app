using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Prism.Mvvm;

namespace Monoregion.App.Entites
{
    public class DirectionViewModel : BindableBase
    {
        public DirectionViewModel(ICommand tappedCommand, ICommand deleteTappedCommand)
        {
            TappedCommand = tappedCommand;
            DeleteTappedCommand = deleteTappedCommand;
        }

        private Guid _id;
        public Guid Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private ObservableCollection<RecordViewModel> _records;
        public ObservableCollection<RecordViewModel> Records
        {
            get => _records;
            set => SetProperty(ref _records, value);
        }

        private ICommand _tappedCommand;
        public ICommand TappedCommand
        {
            get => _tappedCommand;
            set => SetProperty(ref _tappedCommand, value);
        }

        private ICommand _deleteTappedCommand;
        public ICommand DeleteTappedCommand
        {
            get => _deleteTappedCommand;
            set => SetProperty(ref _deleteTappedCommand, value);
        }
    }
}
