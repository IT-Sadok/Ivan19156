using IoT.Application.Common.Mappings;
using IoT.Contracts.Maintenance;
using IoT.Domain.Entities;
using IoT.Interfaces;
using IoT.Interfaces.Mediator;
using IoT.Interfaces.Services;
using IoT.Shared.Common;

namespace IoT.Application.Commands.Maintenance.CreateMaintenanceRecord;

public class CreateMaintenanceRecordCommandHandler(
    IUnitOfWork unitOfWork,
    IEmbeddingService embeddingService) : IRequestHandler<CreateMaintenanceRecordCommand, Result<MaintenanceRecordResponse>>
{
    public async Task<Result<MaintenanceRecordResponse>> Handle(
        CreateMaintenanceRecordCommand request,
        CancellationToken ct)
    {
        var record = new MaintenanceRecord
        {
            Id = Guid.NewGuid(),
            DeviceId = request.DeviceId,
            TechnicianId = request.TechnicianId,
            Notes = request.Notes,
            PerformedAt = request.PerformedAt
        };

        if (!string.IsNullOrWhiteSpace(request.Notes))
            record.NotesEmbedding = await embeddingService.GenerateEmbeddingAsync(request.Notes, ct);

        await unitOfWork.MaintenanceRecords.AddAsync(record, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return Result<MaintenanceRecordResponse>.Success(record.ToResponse());
    }
}