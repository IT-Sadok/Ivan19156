using AutoMapper;
using IoT.Contracts.Devices;
using IoT.Interfaces;
using IoT.Interfaces.Mediator;
using IoT.Shared.Common;

namespace IoT.Application.Commands.Devices.UpdateDevice;

public class UpdateDeviceCommandHandler
    : IRequestHandler<UpdateDeviceCommand, Result<DeviceResponse>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateDeviceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<DeviceResponse>> Handle(
        UpdateDeviceCommand request,
        CancellationToken ct = default)
    {
        var device = await _unitOfWork.Devices.GetByIdAsync(request.Id, ct);

        if (device == null)
            return Result<DeviceResponse>.NotFound($"Device {request.Id} not found");

        device.Name = request.Name;
        device.AdminStatus = request.AdminStatus;
        device.ManufacturerId = request.ManufacturerId;

        await _unitOfWork.Devices.UpdateAsync(device);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result<DeviceResponse>.Success(_mapper.Map<DeviceResponse>(device));
    }
}
