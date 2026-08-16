using DeviceService.Interfaces;
using IoT.Shared.Common;
using IoT.Shared.Mediator;

namespace DeviceService.Application.Commands.UpdateDevice;

public class UpdateDeviceCommandHandler : IRequestHandler<UpdateDeviceCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateDeviceCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result<bool>> ExecuteAsync(UpdateDeviceCommand request, CancellationToken ct = default)
    {
        var device = await _unitOfWork.Devices.GetByIdAsync(request.Id, ct);
        if (device is null)
            return Result<bool>.NotFound($"Device {request.Id} not found.");

        device.Name = request.Name;
        device.AdminStatus = request.AdminStatus;
        device.ManufacturerId = request.ManufacturerId;
        device.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Devices.UpdateAsync(device, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return Result<bool>.Success(true);
    }
}
