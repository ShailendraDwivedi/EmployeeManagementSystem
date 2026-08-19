using EmployeeManagement.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ICacheService _cache;

        public TestController(ICacheService cache)
        {
            _cache = cache;
        }
        [HttpGet]
        public async Task<IActionResult> Test()
        {
            await _cache.SetAsync("test", "Hello Redis");

            var result = await _cache.GetAsync<string>("test");

            return Ok(result);
        }
    }
}
