using DeviceService.Domain.Enums;
using IoT.Shared.Common;
using IoT.Shared.Mediator;

namespace DeviceService.Application.Commands.CreateDevice;

public record CreateDeviceCommand(
    string Name,
    DeviceType Type,
    Guid? ManufacturerId) : IRequest<Result<Guid>>;
