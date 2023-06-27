using Monoregion.App.Entites;
using System.Windows.Input;

namespace Monoregion.App.Extensions
{

    public static class RecordModelExtension
    {
        public static RecordViewModel ToViewModel(this RecordModel model, ICommand tappedCommand, ICommand deleteTappedCommand)
        {
            return new RecordViewModel(tappedCommand, deleteTappedCommand)
            {
                Id = model.Id,
                Name = model.Name,
                DirectionModelId = model.DirectionModelId,
                CreationTime = model.CreationTime,
                Content = model.Content,
            };
        }

        public static RecordModel ToModel(this RecordViewModel model)
        {
            return new RecordModel()
            {
                Id = model.Id,
                Name = model.Name,
                DirectionModelId = model.DirectionModelId,
                CreationTime = model.CreationTime,
                Content = model.Content,
            };
        }
    }
}
