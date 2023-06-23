using Microsoft.AspNetCore.Datasync;
using Microsoft.AspNetCore.Datasync.EFCore;
using Microsoft.AspNetCore.Mvc;
using Monoregion.Web.Entities;

namespace Monoregion.Web.Controllers.TableControllers
{
    [Route("tables/[controller]")]
    public class GlobalEnvironmentVariablesController : TableController<GlobalEnvironmentVariable>
    {
        public GlobalEnvironmentVariablesController(ApplicationDbContext context)
            : base(new EntityTableRepository<GlobalEnvironmentVariable>(context))
        {
            this.Options.PageSize = 128000;
        }
    }
}