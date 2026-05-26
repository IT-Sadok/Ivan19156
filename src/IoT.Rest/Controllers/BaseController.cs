using IoT.Shared.Common;
using Microsoft.AspNetCore.Mvc;

namespace IoT.Rest.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : ControllerBase
{
    protected IActionResult HandleResult<T>(Result<T> result)
    {
        return result.IsSuccess
            ? Ok(result.Value)
            : StatusCode(result.StatusCode, result.Error);
    }

    protected IActionResult HandleCreatedResult<T>(
        Result<T> result,
        string actionName,
        object routeValues)
    {
        return result.IsSuccess
            ? CreatedAtAction(actionName, routeValues, result.Value)
            : StatusCode(result.StatusCode, result.Error);
    }
}