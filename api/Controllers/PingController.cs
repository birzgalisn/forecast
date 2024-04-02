using Microsoft.AspNetCore.Mvc;

namespace Forecast.Controllers;

[ApiController]
[Route("[controller]")]
public class PingController : ControllerBase
{
    [HttpGet(Name = "GetPing")]
    public IActionResult Get()
    {
        return Ok("pong");
    }
}
