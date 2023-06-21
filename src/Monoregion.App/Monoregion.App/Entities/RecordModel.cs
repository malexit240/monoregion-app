using System;
using System.ComponentModel.DataAnnotations;

namespace Monoregion.App.Entites
{
    public class RecordModel
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Content { get; set; }

        public DateTime CreationTime { get; set; }

        public Guid DirectionModelId { get; set; }

        public DirectionModel DirectionModel { get; set; }
    }
}
