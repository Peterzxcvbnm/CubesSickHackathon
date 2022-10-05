using Microsoft.AspNetCore.Mvc;
using SafelogSimulator;

namespace API.Controllers;

[ApiController]
[Route("api")]
public class InstantActionsController : ControllerBase
{
    [HttpPost("instantActions")]
    public async Task<IActionResult> InstantActions(RootDto request)
    {
        
        return Ok();
    }
}