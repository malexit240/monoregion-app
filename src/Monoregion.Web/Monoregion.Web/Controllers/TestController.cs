using Microsoft.AspNetCore.Mvc;
using Monoregion.Web.Entities;

namespace Monoregion.Web.Controllers
{
    public class TestController : Controller
    {
        private ApplicationDbContext _context;
        public TestController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("api/test")]
        public async Task<GlobalEnvironmentVariable> DoSome()
        {
            var variable = new GlobalEnvironmentVariable()
            {
                Key = "Priority",
                Value = "3",
            };

            _context.GlobalEnvironmentVariables.Add(variable);

            await _context.SaveChangesAsync();

            return variable;
        }
    }
}