using Microsoft.AspNetCore.Datasync.EFCore;
using Monoregion.Web.Helpers;

namespace Monoregion.Web.Entities
{
    public abstract class BaseDataStatusObject : EntityTableData
    {
        public BaseDataStatusObject()
        {
            Id = IdGenerationHelper.GetNext();
        }
    }
}
