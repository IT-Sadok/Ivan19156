using IoT.Domain.Enums;
using IoT.Shared.Mediator;
using IoT.Shared.Common;

namespace IoT.Application.Commands.Identity.Register;

public record RegisterCommand(
    string UserName,
    string Email,
    string Password,
    UserRole Role = UserRole.Customer) : IRequest<Result<string>>;
