// Login/LoginCommand.cs
using IoT.Application.DTOs;
using IoT.Interfaces.Mediator;
using IoT.Shared.Common;

namespace IoT.Application.Identity.Commands.Login;

public record LoginCommand(
    string Email,
    string Password) : IRequest<Result<AuthResponse>>;