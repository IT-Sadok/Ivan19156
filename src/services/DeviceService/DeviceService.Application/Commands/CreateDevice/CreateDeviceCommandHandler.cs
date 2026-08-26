using DeviceService.Domain.Entities;
using DeviceService.Interfaces;
using IoT.Shared.Common;
using IoT.Shared.Mediator;

namespace DeviceService.Application.Commands.CreateDevice;

public class CreateDeviceCommandHandler : IRequestHandler<CreateDeviceCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateDeviceCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> ExecuteAsync(CreateDeviceCommand request, CancellationToken ct = default)
    {
        var device = Device.Create(request.Name, request.Type, request.ManufacturerId);
        await _unitOfWork.Devices.AddAsync(device, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return Result<Guid>.Success(device.Id);
    }
}
