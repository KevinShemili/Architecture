using Application.Contracts.Persistence;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Controllers.BaseController;

namespace WebAPI.Controllers
{
    public class TestController : MainControllerBase
    {
        private readonly ICoreDbContext _coreDbContext;

        public TestController(ICoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;
        }

        [HttpPost("clickme")]
        public async Task<IActionResult> ClickMe()
        {
            var entities = await _coreDbContext.EntityAsDbSet<TestEntity>()
                                               .ToListAsync();

            entities.Add(new TestEntity
            {
                TestString = "test"
            });

            _ = await _coreDbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
