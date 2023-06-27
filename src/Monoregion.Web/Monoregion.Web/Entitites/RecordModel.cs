using Microsoft.AspNetCore.Datasync.EFCore;
using System;

namespace Monoregion.Web.Entities
{
    public class RecordModel : DataStatusObject
    {
        public string Name { get; set; }

        public string Content { get; set; }

        public DateTime CreationTime { get; set; }

        public string DirectionModelId { get; set; }

        public DirectionModel DirectionModel { get; set; }
    }
}
