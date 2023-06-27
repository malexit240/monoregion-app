using Microsoft.AspNetCore.Datasync.EFCore;

namespace Monoregion.Web.Entities
{
    public abstract class DataStatusObject : EntityTableData
    {
        public DataStatusObject()
        {
            Id = Guid.NewGuid().ToString("N");
        }
    }

    public class GlobalEnvironmentVariable : DataStatusObject
    {
        public string Key { get; set; }

        public string Value { get; set; }
    }
}
