using Microsoft.AspNetCore.Datasync.EFCore;
using System;
using System.Collections.Generic;

namespace Monoregion.Web.Entities
{
    public class DirectionModel : DataStatusObject
    {
        public string Name { get; set; }

        public List<RecordModel> Records { get; set; }
    }
}
