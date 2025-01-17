using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controller;

[ApiController]
[Route("api/c/platform")]
public class PlatformController : ControllerBase
{
    public PlatformController()
    {
    }

    [HttpPost]
    public ActionResult TestInboundConnection()
    {
        Console.WriteLine("--> Inbound POST # Command Service");

        return Ok("Inbound test of Platform Controller");
    }
}
