using AutoMapper;
using IoT.Contracts.DeviceCommands;
using IoT.Interfaces;
using IoT.Interfaces.Mediator;
using IoT.Shared.Common;

namespace IoT.Application.Queries.DeviceCommands.GetDeviceCommands;

public class GetDeviceCommandsQueryHandler
    : IRequestHandler<GetDeviceCommandsQuery, Result<IEnumerable<DeviceCommandDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetDeviceCommandsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<IEnumerable<DeviceCommandDto>>> Handle(
        GetDeviceCommandsQuery request,
        CancellationToken ct = default)
    {
        var commands = await _unitOfWork.DeviceCommands
            .GetByDeviceIdAsync(request.DeviceId);

        return Result<IEnumerable<DeviceCommandDto>>.Success(
            _mapper.Map<IEnumerable<DeviceCommandDto>>(commands));
    }
}
