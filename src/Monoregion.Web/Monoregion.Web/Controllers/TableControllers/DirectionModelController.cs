using Microsoft.AspNetCore.Datasync;
using Microsoft.AspNetCore.Datasync.EFCore;
using Microsoft.AspNetCore.Mvc;
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

        public override async Task<IActionResult> ReplaceAsync([FromRoute] string id, [FromBody] DirectionModel item, CancellationToken token = default)
        {
            var storedDirectory = await Repository.ReadAsync(id);
            if (storedDirectory.UpdatedAt < item.UpdatedAt)
            {
                await Repository.ReplaceAsync(item);
            }

            return Ok(item);
        }
    }
}