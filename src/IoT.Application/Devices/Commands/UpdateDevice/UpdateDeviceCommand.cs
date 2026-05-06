// IoT.Application/Devices/Commands/UpdateDevice/UpdateDeviceCommand.cs
using IoT.Application.DTOs;
using IoT.Domain.Enums;
using IoT.Interfaces.Mediator;
using IoT.Shared.Common;

namespace IoT.Application.Devices.Commands.UpdateDevice;

public record UpdateDeviceCommand(
    Guid Id,
    string Name,
    DeviceAdminStatus AdminStatus,
    Guid? ManufacturerId) : IRequest<Result<DeviceDto>>;