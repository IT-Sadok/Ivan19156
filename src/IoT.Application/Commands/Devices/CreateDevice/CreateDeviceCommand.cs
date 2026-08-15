using IoT.Contracts.Devices;
using IoT.Domain.Enums;
using IoT.Shared.Mediator;
using IoT.Shared.Common;

namespace IoT.Application.Commands.Devices.CreateDevice;

public record CreateDeviceCommand(
    string Name,
    DeviceType Type,
    Guid? ManufacturerId) : IRequest<Result<DeviceResponse>>;
