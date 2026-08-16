using DeviceService.Application.Common.Mappings;
using DeviceService.Interfaces;
using IoT.Contracts.Devices;
using IoT.Shared.Common;
using IoT.Shared.Mediator;

namespace DeviceService.Application.Queries.GetDeviceById;

public class GetDeviceByIdQueryHandler : IRequestHandler<GetDeviceByIdQuery, Result<DeviceResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetDeviceByIdQueryHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result<DeviceResponse>> ExecuteAsync(GetDeviceByIdQuery request, CancellationToken ct = default)
    {
        var device = await _unitOfWork.Devices.GetByIdAsync(request.Id, ct);
        if (device is null)
            return Result<DeviceResponse>.NotFound($"Device {request.Id} not found.");

        return Result<DeviceResponse>.Success(device.ToDto());
    }
}
