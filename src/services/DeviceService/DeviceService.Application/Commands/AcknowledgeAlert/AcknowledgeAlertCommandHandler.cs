using DeviceService.Domain.Entities;
using DeviceService.Domain.Enums;
using DeviceService.Interfaces;
using IoT.Shared.Common;
using IoT.Shared.Mediator;

namespace DeviceService.Application.Commands.AcknowledgeAlert;

public class AcknowledgeAlertCommandHandler : IRequestHandler<AcknowledgeAlertCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public AcknowledgeAlertCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result<bool>> ExecuteAsync(AcknowledgeAlertCommand request, CancellationToken ct = default)
    {
        var alert = await _unitOfWork.Alerts.GetByIdAsync(request.AlertId, ct);
        if (alert is null)
            return Result<bool>.NotFound($"Alert {request.AlertId} not found.");

        var acknowledgement = new AlertAcknowledgement
        {
            AlertId = request.AlertId,
            AcknowledgedById = request.AcknowledgedById,
            AcknowledgedAt = DateTime.UtcNow,
            Note = request.Note,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        alert.Acknowledgements.Add(acknowledgement);
        alert.Status = AlertStatus.Acknowledged;
        alert.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Alerts.UpdateAsync(alert, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return Result<bool>.Success(true);
    }
}
