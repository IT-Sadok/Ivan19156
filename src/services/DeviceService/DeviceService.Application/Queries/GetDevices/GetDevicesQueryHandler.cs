using DeviceService.Application.Common.Mappings;
using DeviceService.Interfaces;
using IoT.Contracts.Devices;
using IoT.Shared.Common;
using IoT.Shared.Mediator;

namespace DeviceService.Application.Queries.GetDevices;

public class GetDevicesQueryHandler : IRequestHandler<GetDevicesQuery, Result<PagedResult<DeviceResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetDevicesQueryHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result<PagedResult<DeviceResponse>>> ExecuteAsync(GetDevicesQuery request, CancellationToken ct = default)
    {
        var (items, totalCount) = await _unitOfWork.Devices.GetPagedAsync(
            request.Page, request.PageSize, request.Type, ct);

        var result = new PagedResult<DeviceResponse>
        {
            Items = items.Select(d => d.ToDto()).ToList(),
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };

        return Result<PagedResult<DeviceResponse>>.Success(result);
    }
}
