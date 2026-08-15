using IoT.Application.Common.Mappings;
using IoT.Contracts.DeviceCommands;
using IoT.Interfaces;
using IoT.Shared.Mediator;
using IoT.Interfaces.Services;
using IoT.Shared.Common;

namespace IoT.Application.Queries.DeviceCommands.GetDeviceCommands;

public class GetDeviceCommandsQueryHandler
    : IRequestHandler<GetDeviceCommandsQuery, Result<IEnumerable<DeviceCommandResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cache;

    public GetDeviceCommandsQueryHandler(IUnitOfWork unitOfWork, ICacheService cache)
    {
        _unitOfWork = unitOfWork;
        _cache = cache;
    }

    public async Task<Result<IEnumerable<DeviceCommandResponse>>> ExecuteAsync(
        GetDeviceCommandsQuery request,
        CancellationToken ct = default)
    {
        var cacheKey = $"device:commands:{request.DeviceId}";

        var cached = await _cache.GetAsync<IEnumerable<DeviceCommandResponse>>(cacheKey);
        if (cached != null)
            return Result<IEnumerable<DeviceCommandResponse>>.Success(cached);

        var commands = await _unitOfWork.DeviceCommands
            .GetByDeviceIdAsync(request.DeviceId, ct);

        var dtos = commands.Select(c => c.ToResponse());

        await _cache.SetAsync(cacheKey, dtos, TimeSpan.FromMinutes(2));

        return Result<IEnumerable<DeviceCommandResponse>>.Success(dtos);
    }
}
