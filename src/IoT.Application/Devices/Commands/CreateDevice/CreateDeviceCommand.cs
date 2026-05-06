// IoT.Application/Devices/Commands/CreateDevice/CreateDeviceCommand.cs
using IoT.Application.DTOs;
using IoT.Interfaces.Mediator;
using IoT.Shared.Common;
using IoT.Domain.Enums;

namespace IoT.Application.Devices.Commands.CreateDevice;

public record CreateDeviceCommand(
    string Name,
    DeviceType Type,
    Guid? ManufacturerId) : IRequest<Result<DeviceDto>>;