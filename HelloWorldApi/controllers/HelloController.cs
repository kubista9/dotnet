using Microsoft.AspNetCore.Mvc;

namespace HelloWorldApi.controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HelloController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetHello()
        {
            return Ok("Hello");
        }
    }
}
