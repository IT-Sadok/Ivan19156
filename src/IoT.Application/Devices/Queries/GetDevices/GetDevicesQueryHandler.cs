// IoT.Application/Devices/Queries/GetDevices/GetDevicesQueryHandler.cs
using IoT.Application.DTOs;
using IoT.Interfaces;
using IoT.Interfaces.Mediator;
using IoT.Shared.Common;

namespace IoT.Application.Devices.Queries.GetDevices;

public class GetDevicesQueryHandler
    : IRequestHandler<GetDevicesQuery, Result<IEnumerable<DeviceDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetDevicesQueryHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<IEnumerable<DeviceDto>>> Handle(
        GetDevicesQuery request,
        CancellationToken ct = default)
    {
        var devices = await _unitOfWork.Devices.GetAllAsync();

        var dtos = devices.Select(d => new DeviceDto(
            d.Id,
            d.Name,
            d.Type,
            d.AdminStatus,
            d.LastSeen,
            d.Manufacturer?.Name,
            d.CreatedAt
        ));

        return Result<IEnumerable<DeviceDto>>.Success(dtos);
    }
}