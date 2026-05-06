// IoT.Application/Devices/Queries/GetDeviceById/GetDeviceByIdQueryHandler.cs
using IoT.Application.DTOs;
using IoT.Interfaces;
using IoT.Interfaces.Mediator;
using IoT.Shared.Common;

namespace IoT.Application.Devices.Queries.GetDeviceById;

public class GetDeviceByIdQueryHandler
    : IRequestHandler<GetDeviceByIdQuery, Result<DeviceDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetDeviceByIdQueryHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<DeviceDto>> Handle(
        GetDeviceByIdQuery request,
        CancellationToken ct = default)
    {
        var device = await _unitOfWork.Devices.GetWithDetailsAsync(request.Id);

        if (device == null)
            return Result<DeviceDto>.NotFound($"Device {request.Id} not found");

        return Result<DeviceDto>.Success(new DeviceDto(
            device.Id,
            device.Name,
            device.Type,
            device.AdminStatus,
            device.LastSeen,
            device.Manufacturer?.Name,
            device.CreatedAt
        ));
    }
}