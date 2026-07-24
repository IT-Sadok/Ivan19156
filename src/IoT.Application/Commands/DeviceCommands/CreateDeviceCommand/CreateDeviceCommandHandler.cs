using IoT.Application.Common.Mappings;
using IoT.Contracts.DeviceCommands;
using IoT.Domain.Entities;
using IoT.Domain.Enums;
using IoT.Interfaces;
using IoT.Interfaces.Mediator;
using IoT.Shared.Common;

namespace IoT.Application.Commands.DeviceCommands.CreateDeviceCommand;

public class CreateDeviceCommandHandler
    : IRequestHandler<CreateDeviceCommandCommand, Result<DeviceCommandResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateDeviceCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<DeviceCommandResponse>> Handle(
        CreateDeviceCommandCommand request,
        CancellationToken ct = default)
    {
        var device = await _unitOfWork.Devices.GetByIdAsync(request.DeviceId, ct);
        if (device == null)
            return Result<DeviceCommandResponse>.NotFound($"Device {request.DeviceId} not found");

        var slug = request.CommandTypeSlug.ToSlug();
        var commandType = await _unitOfWork.CommandTypes
            .FirstOrDefaultAsync(ct2 => ct2.Slug == slug, ct: ct);
        if (commandType == null)
            return Result<DeviceCommandResponse>.NotFound($"CommandType '{slug}' not found");

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

        await _unitOfWork.DeviceCommands.AddAsync(command, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result<DeviceCommandResponse>.Success(command.ToResponse());
    }
}
