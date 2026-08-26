using IoT.Contracts.Events;
using IoT.Contracts.Maintenance;
using IoT.Shared.Common;
using IoT.Shared.Mediator;
using MaintenanceService.Application.Common.Mappings;
using MaintenanceService.Domain.Entities;
using MaintenanceService.Domain.ValueObjects;
using MaintenanceService.Interfaces;
using MassTransit;

namespace MaintenanceService.Application.Commands.CreateMaintenanceRecord;

public class CreateMaintenanceRecordCommandHandler(
    IUnitOfWork unitOfWork,
    ITopicProducer<MaintenanceRecordCreatedEvent> producer)
    : IRequestHandler<CreateMaintenanceRecordCommand, Result<MaintenanceRecordResponse>>
{
    public async Task<Result<MaintenanceRecordResponse>> ExecuteAsync(
        CreateMaintenanceRecordCommand request,
        CancellationToken ct)
    {
        var record = MaintenanceRecord.Create(
            DeviceId.From(request.DeviceId),
            TechnicianId.From(request.TechnicianId),
            request.Notes,
            request.PerformedAt);
        await unitOfWork.MaintenanceRecords.AddAsync(record, ct);
        await unitOfWork.SaveChangesAsync(ct);

        if (!string.IsNullOrWhiteSpace(request.Notes))
        {
            await producer.Produce(new MaintenanceRecordCreatedEvent
            {
                RecordId = record.Id,
                Notes = request.Notes
            }, ct);
        }

        return Result<MaintenanceRecordResponse>.Success(record.ToResponse());
    }
}