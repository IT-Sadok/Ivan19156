using DeviceService.Interfaces;
using IoT.Shared.Common;
using IoT.Shared.Mediator;

namespace DeviceService.Application.Commands.DeleteDevice;

public class DeleteDeviceCommandHandler : IRequestHandler<DeleteDeviceCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteDeviceCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result<bool>> ExecuteAsync(DeleteDeviceCommand request, CancellationToken ct = default)
    {
        var device = await _unitOfWork.Devices.GetByIdAsync(request.Id, ct);
        if (device is null)
            return Result<bool>.NotFound($"Device {request.Id} not found.");

        device.Delete();
        device.ClearDomainEvents();

        await _unitOfWork.Devices.DeleteAsync(device, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return Result<bool>.Success(true);
    }
}
