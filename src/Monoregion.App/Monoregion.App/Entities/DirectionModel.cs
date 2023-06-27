using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Monoregion.App.Entites
{
    [Table("directionmodel")]
    public class DirectionModel : DataStatusObject
    {
        public string Name { get; set; }

        [NotMapped]
        public List<RecordModel> Records { get; set; }
    }
}
