using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

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
