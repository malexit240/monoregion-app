﻿using Microsoft.AspNetCore.Datasync;
using Microsoft.AspNetCore.Datasync.EFCore;
using Microsoft.AspNetCore.Mvc;
using Monoregion.Web.Entities;

namespace Monoregion.Web.Controllers.TableControllers
{
    [Route("tables/[controller]")]
    public class RecordModelController : TableController<RecordModel>
    {
        public RecordModelController(ApplicationDbContext context)
            : base(new EntityTableRepository<RecordModel>(context))
        {
            this.Options.PageSize = 128000;
        }

        public override async Task<IActionResult> ReplaceAsync([FromRoute] string id, [FromBody] RecordModel item, CancellationToken token = default)
        {
            await Repository.ReplaceAsync(item);
            return Ok(item);
        }
    }
}