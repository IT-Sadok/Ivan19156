using IoT.Interfaces;
using IoT.Shared.Mediator;
using IoT.Shared.Common;

namespace IoT.Application.Commands.Devices.DeleteDevice;

public class DeleteDeviceCommandHandler
    : IRequestHandler<DeleteDeviceCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteDeviceCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<bool>> ExecuteAsync(
        DeleteDeviceCommand request,
        CancellationToken ct = default)
    {
        var device = await _unitOfWork.Devices.GetByIdAsync(request.Id, ct);

        if (device == null)
            return Result<bool>.NotFound($"Device {request.Id} not found");

        await _unitOfWork.Devices.DeleteAsync(device);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result<bool>.Success(true);
    }
}
