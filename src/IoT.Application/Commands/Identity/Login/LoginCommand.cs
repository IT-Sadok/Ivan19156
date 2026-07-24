using IoT.Contracts.Identity;
using IoT.Interfaces.Mediator;
using IoT.Shared.Common;

namespace IoT.Application.Commands.Identity.Login;

public record LoginCommand(
    string Email,
    string Password) : IRequest<Result<AuthResponse>>;
