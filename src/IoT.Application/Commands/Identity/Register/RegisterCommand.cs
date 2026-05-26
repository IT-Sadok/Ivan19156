using IoT.Interfaces.Mediator;
using IoT.Shared.Common;

namespace IoT.Application.Commands.Identity.Register;

public record RegisterCommand(
    string UserName,
    string Email,
    string Password) : IRequest<Result<string>>;
