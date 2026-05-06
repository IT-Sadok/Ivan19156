// IoT.Application/Devices/Commands/UpdateDevice/UpdateDeviceCommandHandler.cs
using IoT.Application.DTOs;
using IoT.Interfaces;
using IoT.Interfaces.Mediator;
using IoT.Shared.Common;

namespace IoT.Application.Devices.Commands.UpdateDevice;

public class UpdateDeviceCommandHandler
    : IRequestHandler<UpdateDeviceCommand, Result<DeviceDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateDeviceCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<DeviceDto>> Handle(
        UpdateDeviceCommand request,
        CancellationToken ct = default)
    {
        var device = await _unitOfWork.Devices.GetByIdAsync(request.Id);

        if (device == null)
            return Result<DeviceDto>.NotFound($"Device {request.Id} not found");

        device.Name = request.Name;
        device.AdminStatus = request.AdminStatus;
        device.ManufacturerId = request.ManufacturerId;

        await _unitOfWork.Devices.UpdateAsync(device);
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