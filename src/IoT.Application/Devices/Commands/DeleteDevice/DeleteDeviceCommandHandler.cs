// IoT.Application/Devices/Commands/DeleteDevice/DeleteDeviceCommandHandler.cs
using IoT.Interfaces;
using IoT.Interfaces.Mediator;
using IoT.Shared.Common;

namespace IoT.Application.Devices.Commands.DeleteDevice;

public class DeleteDeviceCommandHandler
    : IRequestHandler<DeleteDeviceCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteDeviceCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<bool>> Handle(
        DeleteDeviceCommand request,
        CancellationToken ct = default)
    {
        var device = await _unitOfWork.Devices.GetByIdAsync(request.Id);

        if (device == null)
            return Result<bool>.NotFound($"Device {request.Id} not found");

        await _unitOfWork.Devices.DeleteAsync(device);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result<bool>.Success(true);
    }
}