using AutoMapper;
using IoT.Contracts.DeviceCommands;
using IoT.Domain.Entities;
using IoT.Domain.Enums;
using IoT.Interfaces;
using IoT.Interfaces.Mediator;
using IoT.Shared.Common;

namespace IoT.Application.Commands.DeviceCommands.CreateDeviceCommand;

public class CreateDeviceCommandHandler
    : IRequestHandler<CreateDeviceCommandCommand, Result<DeviceCommandDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateDeviceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<DeviceCommandDto>> Handle(
        CreateDeviceCommandCommand request,
        CancellationToken ct = default)
    {
        var device = await _unitOfWork.Devices.GetByIdAsync(request.DeviceId);
        if (device == null)
            return Result<DeviceCommandDto>.NotFound($"Device {request.DeviceId} not found");

        var commandType = await _unitOfWork.CommandTypes
            .FirstOrDefaultAsync(ct2 => ct2.Slug == request.CommandTypeSlug);
        if (commandType == null)
            return Result<DeviceCommandDto>.NotFound($"CommandType {request.CommandTypeSlug} not found");

        var command = new DeviceCommand
        {
            Id = Guid.NewGuid(),
            DeviceId = request.DeviceId,
            CommandTypeId = commandType.Id,
            CommandType = commandType,
            Parameters = request.Parameters,
            Priority = request.Priority,
            Status = CommandStatus.Created,
            ExpiresAt = request.ExpiresAt
        };

        await _unitOfWork.DeviceCommands.AddAsync(command);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result<DeviceCommandDto>.Success(_mapper.Map<DeviceCommandDto>(command));
    }
}
