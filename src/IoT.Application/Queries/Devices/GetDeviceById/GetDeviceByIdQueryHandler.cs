using AutoMapper;
using IoT.Contracts.Devices;
using IoT.Interfaces;
using IoT.Interfaces.Mediator;
using IoT.Interfaces.Services;
using IoT.Shared.Common;

namespace IoT.Application.Queries.Devices.GetDeviceById;

public class GetDeviceByIdQueryHandler
    : IRequestHandler<GetDeviceByIdQuery, Result<DeviceResponse>>
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

    public async Task<Result<DeviceResponse>> Handle(
        GetDeviceByIdQuery request,
        CancellationToken ct = default)
    {
        var cacheKey = $"devices:{request.Id}";

        var cached = await _cache.GetAsync<DeviceResponse>(cacheKey);
        if (cached != null)
            return Result<DeviceResponse>.Success(cached);

        var device = await _unitOfWork.Devices.GetWithDetailsAsync(request.Id, ct);

        if (device == null)
            return Result<DeviceResponse>.NotFound($"Device {request.Id} not found");

        var dto = _mapper.Map<DeviceResponse>(device);

        await _cache.SetAsync(cacheKey, dto, TimeSpan.FromMinutes(5));

        return Result<DeviceResponse>.Success(dto);
    }
}