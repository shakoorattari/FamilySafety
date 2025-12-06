using Microsoft.AspNetCore.Mvc;

namespace FamilySafety.API.Controllers;

/// <summary>
/// Base controller with common functionality
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    protected ActionResult HandleFailure(string error)
    {
        return BadRequest(new { Error = error });
    }

    protected ActionResult HandleFailure(IEnumerable<string> errors)
    {
        return BadRequest(new { Errors = errors });
    }
}
