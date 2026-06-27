using IoT.Application.Common.Mappings;
using IoT.Contracts.Events;
using IoT.Contracts.Maintenance;
using IoT.Interfaces;
using IoT.Interfaces.Mediator;
using IoT.Interfaces.Services;
using IoT.Shared.Common;
using MassTransit;

namespace IoT.Application.Commands.Maintenance.CreateMaintenanceRecord;

public class CreateMaintenanceRecordCommandHandler(
    IUnitOfWork unitOfWork,
    ITopicProducer<MaintenanceRecordCreatedEvent> producer)
    : IRequestHandler<CreateMaintenanceRecordCommand, Result<MaintenanceRecordResponse>>
{
    public async Task<Result<MaintenanceRecordResponse>> Handle(
        CreateMaintenanceRecordCommand request,
        CancellationToken ct)
    {
        var record = request.ToEntity();

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