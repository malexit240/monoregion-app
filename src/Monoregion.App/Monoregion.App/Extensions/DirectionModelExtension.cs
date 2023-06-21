using System;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using Monoregion.App.Entites;

namespace Monoregion.App.Extensions
{
    public static class DirectionModelExtension
    {
        public static DirectionViewModel ToViewModel(this DirectionModel model, ICommand tappedCommand, ICommand deleteTappedCommand)
        {
            return new DirectionViewModel(tappedCommand, deleteTappedCommand)
            {
                Id = model.Id,
                Name = model.Name,
                Records = new ObservableCollection<RecordViewModel>(model.Records?.Select(r => r.ToViewModel(null, null)) ?? new List<RecordViewModel>()),
            };
        }

        public static DirectionModel ToModel(this DirectionViewModel viewModel)
        {
            return new DirectionModel()
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                Records = viewModel.Records is null
                ? null
                : new List<RecordModel>(viewModel.Records.Select(r => r.ToModel()))
            };
        }
    }
}
