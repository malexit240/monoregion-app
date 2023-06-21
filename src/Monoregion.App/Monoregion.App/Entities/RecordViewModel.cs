using System;
using System.Windows.Input;
using Prism.Mvvm;

namespace Monoregion.App.Entites
{
    public class RecordViewModel : BindableBase
    {
        public RecordViewModel(ICommand tappedCommand, ICommand deleteTappedCommand)
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

        private string _content;
        public string Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        private DateTime _creationTime;
        public DateTime CreationTime
        {
            get => _creationTime;
            set => SetProperty(ref _creationTime, value);
        }

        private Guid _directionModelid;
        public Guid DirectionModelId
        {
            get => _directionModelid;
            set => SetProperty(ref _directionModelid, value);
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
