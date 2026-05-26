using AutoMapper;
using IoT.Contracts.Devices;
using IoT.Domain.Enums;
using IoT.Interfaces;
using IoT.Interfaces.Mediator;
using IoT.Interfaces.Services;
using IoT.Shared.Common;
using Microsoft.EntityFrameworkCore;

namespace IoT.Application.Queries.Devices.GetDevices;

public class GetDevicesQueryHandler
    : IRequestHandler<GetDevicesQuery, Result<PagedResult<DeviceResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ICacheService _cache;
    private const string CacheKeyPrefix = "devices:all";

    public GetDevicesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICacheService cache)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<Result<PagedResult<DeviceResponse>>> Handle(
        GetDevicesQuery request,
        CancellationToken ct = default)
    {
        var cacheKey = $"{CacheKeyPrefix}:p{request.Page}:ps{request.PageSize}:t{request.Type}:s{request.AdminStatus}:m{request.ManufacturerId}";

        var cached = await _cache.GetAsync<PagedResult<DeviceResponse>>(cacheKey);
        if (cached != null)
            return Result<PagedResult<DeviceResponse>>.Success(cached);

        var query = _unitOfWork.Devices.Filter(noTracking: true);

        if (request.Type.HasValue)
            query = query.Where(d => d.Type == (DeviceType)request.Type.Value);

        if (request.AdminStatus.HasValue)
            query = query.Where(d => d.AdminStatus == (DeviceAdminStatus)request.AdminStatus.Value);

        if (request.ManufacturerId.HasValue)
            query = query.Where(d => d.ManufacturerId == request.ManufacturerId.Value);

        var totalCount = await query.CountAsync(ct);

        var devices = await query
            .Include(d => d.Manufacturer)
            .OrderBy(d => d.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(ct);

        var result = new PagedResult<DeviceResponse>
        {
            Items = _mapper.Map<IEnumerable<DeviceResponse>>(devices),
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };

        await _cache.SetAsync(cacheKey, result, TimeSpan.FromMinutes(5));

        return Result<PagedResult<DeviceResponse>>.Success(result);
    }
}