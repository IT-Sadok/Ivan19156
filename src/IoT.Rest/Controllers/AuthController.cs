using IoT.Application.Common.Mappings;
using IoT.Contracts.Identity;
using IoT.Interfaces.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace IoT.Rest.Controllers;

[Route("api/auth")]
public class AuthController : BaseController
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
        => _mediator = mediator;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        => HandleResult(await _mediator.Send(request.ToCommand()));

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
        => HandleResult(await _mediator.Send(request.ToCommand()));
}
