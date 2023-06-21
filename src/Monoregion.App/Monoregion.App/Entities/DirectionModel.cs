using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Monoregion.App.Entites
{
    public class DirectionModel
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<RecordModel> Records { get; set; }
    }
}
