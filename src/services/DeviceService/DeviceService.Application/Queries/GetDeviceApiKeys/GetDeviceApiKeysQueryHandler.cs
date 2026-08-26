using DeviceService.Interfaces;
using IoT.Shared.Common;
using IoT.Shared.Mediator;

namespace DeviceService.Application.Queries.GetDeviceApiKeys;

public class GetDeviceApiKeysQueryHandler : IRequestHandler<GetDeviceApiKeysQuery, Result<IEnumerable<ApiKeyDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetDeviceApiKeysQueryHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result<IEnumerable<ApiKeyDto>>> ExecuteAsync(GetDeviceApiKeysQuery request, CancellationToken ct = default)
    {
        var keys = await _unitOfWork.DeviceApiKeys.GetByDeviceIdAsync(request.DeviceId, ct);
        var mapped = keys.Select(k => new ApiKeyDto(k.Id, k.Prefix, k.IsActive, k.ExpiresAt));
        return Result<IEnumerable<ApiKeyDto>>.Success(mapped);
    }
}
