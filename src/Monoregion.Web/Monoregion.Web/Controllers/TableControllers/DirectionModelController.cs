using Microsoft.AspNetCore.Datasync;
using Microsoft.AspNetCore.Datasync.EFCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Monoregion.Web.Entities;

namespace Monoregion.Web.Controllers.TableControllers
{

    [Route("tables/[controller]")]
    public class DirectionModelController : TableController<DirectionModel>
    {
        public DirectionModelController(ApplicationDbContext context)
            : base(new EntityTableRepository<DirectionModel>(context))
        {
            this.Options.PageSize = 128000;
        }
    }
}