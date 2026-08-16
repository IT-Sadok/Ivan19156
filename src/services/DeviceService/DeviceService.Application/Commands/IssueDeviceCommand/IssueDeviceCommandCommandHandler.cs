using DeviceService.Domain.Entities;
using DeviceService.Domain.Enums;
using DeviceService.Interfaces;
using IoT.Shared.Common;
using IoT.Shared.Mediator;

namespace DeviceService.Application.Commands.IssueDeviceCommand;

public class IssueDeviceCommandCommandHandler : IRequestHandler<IssueDeviceCommandCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;

    public IssueDeviceCommandCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> ExecuteAsync(IssueDeviceCommandCommand request, CancellationToken ct = default)
    {
        var device = await _unitOfWork.Devices.GetByIdAsync(request.DeviceId, ct);
        if (device is null)
            return Result<Guid>.NotFound($"Device {request.DeviceId} not found.");

        var command = new DeviceCommand
        {
            DeviceId = request.DeviceId,
            CommandTypeId = request.CommandTypeId,
            IssuedById = request.IssuedById,
            Payload = request.Payload,
            Status = CommandStatus.Created,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _unitOfWork.DeviceCommands.AddAsync(command, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return Result<Guid>.Success(command.Id);
    }
}
