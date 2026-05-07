using AutoMapper;
using IoT.Contracts.Devices;
using IoT.Interfaces;
using IoT.Interfaces.Mediator;
using IoT.Interfaces.Services;
using IoT.Shared.Common;

namespace IoT.Application.Queries.Devices.GetDeviceById;

public class GetDeviceByIdQueryHandler
    : IRequestHandler<GetDeviceByIdQuery, Result<DeviceDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICacheService _cache;

    public GetDeviceByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cache)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<Result<DeviceDto>> Handle(
        GetDeviceByIdQuery request,
        CancellationToken ct = default)
    {
        var cacheKey = $"devices:{request.Id}";

        var cached = await _cache.GetAsync<DeviceDto>(cacheKey);
        if (cached != null)
            return Result<DeviceDto>.Success(cached);

        var device = await _unitOfWork.Devices.GetWithDetailsAsync(request.Id);

        if (device == null)
            return Result<DeviceDto>.NotFound($"Device {request.Id} not found");

        var dto = _mapper.Map<DeviceDto>(device);

        await _cache.SetAsync(cacheKey, dto, TimeSpan.FromMinutes(5));

        return Result<DeviceDto>.Success(dto);
    }
}