using AutoMapper;
using IoT.Application.Commands.Identity.Login;
using IoT.Application.Commands.Identity.Register;
using IoT.Contracts.Identity;
using IoT.Interfaces.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace IoT.Rest.Controllers;

[Route("api/auth")]
public class AuthController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public AuthController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        => HandleResult(await _mediator.Send(_mapper.Map<RegisterCommand>(request)));

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
        => HandleResult(await _mediator.Send(_mapper.Map<LoginCommand>(request)));
}
