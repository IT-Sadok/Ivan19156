using IoT.Shared.Common;
using IoT.Shared.Mediator;

namespace DeviceService.Application.Commands.IssueDeviceCommand;

public record IssueDeviceCommandCommand(
    Guid DeviceId,
    Guid CommandTypeId,
    Guid? IssuedById,
    string? Payload) : IRequest<Result<Guid>>;
