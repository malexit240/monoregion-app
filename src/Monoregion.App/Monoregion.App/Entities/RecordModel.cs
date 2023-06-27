using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Monoregion.App.Entites
{
    [Table("recordmodel")]
    public class RecordModel : DataStatusObject
    {
        public string Name { get; set; }

        public string Content { get; set; }

        public DateTime CreationTime { get; set; }

        public string DirectionModelId { get; set; }

        [NotMapped]
        public DirectionModel DirectionModel { get; set; }
    }
}
