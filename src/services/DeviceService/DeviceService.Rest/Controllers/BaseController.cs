using IoT.Shared.Common;
using Microsoft.AspNetCore.Mvc;

namespace DeviceService.Rest.Controllers;

[ApiController]
public abstract class BaseController : ControllerBase
{
    protected IActionResult HandleResult<T>(Result<T> result)
        => result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
}
