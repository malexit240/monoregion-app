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
        public async Task<string> GetSome()
        {
            return "some result";
        }

        [HttpPost("api/test")]
        public async Task<DirectionModel> DoSome()
        {
            var direction = new DirectionModel()
            {
                Name = "My title",
            };

            _context.Directions.Add(direction);

            await _context.SaveChangesAsync();

            return direction;
        }
    }
}