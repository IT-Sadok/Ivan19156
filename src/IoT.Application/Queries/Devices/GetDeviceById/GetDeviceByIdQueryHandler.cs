using AutoMapper;
using IoT.Contracts.Devices;
using IoT.Interfaces;
using IoT.Interfaces.Mediator;
using IoT.Shared.Common;

namespace IoT.Application.Queries.Devices.GetDeviceById;

public class GetDeviceByIdQueryHandler
    : IRequestHandler<GetDeviceByIdQuery, Result<DeviceDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetDeviceByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<DeviceDto>> Handle(
        GetDeviceByIdQuery request,
        CancellationToken ct = default)
    {
        var device = await _unitOfWork.Devices.GetWithDetailsAsync(request.Id);

        if (device == null)
            return Result<DeviceDto>.NotFound($"Device {request.Id} not found");

        return Result<DeviceDto>.Success(_mapper.Map<DeviceDto>(device));
    }
}
