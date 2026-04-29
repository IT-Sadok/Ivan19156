using IoT.Application.Identity.Commands.Login;
using IoT.Application.Identity.Commands.Register;
using IoT.Interfaces.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace IoT.Rest.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
        => _mediator = mediator;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess
            ? Ok(result.Value)
            : StatusCode(result.StatusCode, result.Error);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var result = await _mediator.Send(command);
        return result.IsSuccess
            ? Ok(result.Value)
            : StatusCode(result.StatusCode, result.Error);
    }
}