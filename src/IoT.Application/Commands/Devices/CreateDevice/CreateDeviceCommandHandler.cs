using IoT.Application.Common.Mappings;
using IoT.Contracts.Devices;
using IoT.Domain.Entities;
using IoT.Domain.Enums;
using IoT.Interfaces;
using IoT.Shared.Mediator;
using IoT.Shared.Common;

namespace IoT.Application.Commands.Devices.CreateDevice;

public class CreateDeviceCommandHandler
    : IRequestHandler<CreateDeviceCommand, Result<DeviceResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateDeviceCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<DeviceResponse>> ExecuteAsync(
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

        await _unitOfWork.Devices.AddAsync(device, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result<DeviceResponse>.Success(device.ToResponse());
    }
}
