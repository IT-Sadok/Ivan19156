using AutoMapper;
using IoT.Contracts.Devices;
using IoT.Domain.Enums;
using IoT.Interfaces;
using IoT.Interfaces.Mediator;
using IoT.Shared.Common;
using Microsoft.EntityFrameworkCore;

namespace IoT.Application.Queries.Devices.GetDevices;

public class GetDevicesQueryHandler
    : IRequestHandler<GetDevicesQuery, Result<PagedResult<DeviceDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetDevicesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<PagedResult<DeviceDto>>> Handle(
        GetDevicesQuery request,
        CancellationToken ct = default)
    {
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

        return Result<PagedResult<DeviceDto>>.Success(new PagedResult<DeviceDto>
        {
            Items = _mapper.Map<IEnumerable<DeviceDto>>(devices),
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        });
    }
}
