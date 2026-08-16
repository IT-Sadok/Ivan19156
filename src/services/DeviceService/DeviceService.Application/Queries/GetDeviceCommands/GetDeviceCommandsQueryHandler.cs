using DeviceService.Application.Common.Mappings;
using DeviceService.Interfaces;
using IoT.Contracts.DeviceCommands;
using IoT.Shared.Common;
using IoT.Shared.Mediator;

namespace DeviceService.Application.Queries.GetDeviceCommands;

public class GetDeviceCommandsQueryHandler : IRequestHandler<GetDeviceCommandsQuery, Result<IEnumerable<DeviceCommandResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetDeviceCommandsQueryHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result<IEnumerable<DeviceCommandResponse>>> ExecuteAsync(GetDeviceCommandsQuery request, CancellationToken ct = default)
    {
        var commands = await _unitOfWork.DeviceCommands.GetByDeviceIdAsync(request.DeviceId, ct);
        var mapped = commands.Select(c => c.ToDto());
        return Result<IEnumerable<DeviceCommandResponse>>.Success(mapped);
    }
}
