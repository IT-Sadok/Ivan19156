// IoT.Application/Devices/Commands/CreateDevice/CreateDeviceCommandHandler.cs
using IoT.Application.DTOs;
using IoT.Domain.Entities;
using IoT.Domain.Enums;
using IoT.Interfaces;
using IoT.Interfaces.Mediator;
using IoT.Shared.Common;

namespace IoT.Application.Devices.Commands.CreateDevice;

public class CreateDeviceCommandHandler
    : IRequestHandler<CreateDeviceCommand, Result<DeviceDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateDeviceCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<DeviceDto>> Handle(
        CreateDeviceCommand request,
        CancellationToken ct = default)
    {
        var device = new Device
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Type = request.Type,
            ManufacturerId = request.ManufacturerId,
            AdminStatus = DeviceAdminStatus.Active
        };

        await _unitOfWork.Devices.AddAsync(device);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result<DeviceDto>.Success(new DeviceDto(
            device.Id,
            device.Name,
            device.Type,
            device.AdminStatus,
            device.LastSeen,
            null,
            device.CreatedAt
        ));
    }
}