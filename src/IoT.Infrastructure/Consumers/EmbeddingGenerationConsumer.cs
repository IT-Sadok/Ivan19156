using IoT.Contracts.Events;
using IoT.Interfaces;
using IoT.Interfaces.Services;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace IoT.Infrastructure.Consumers;

public class EmbeddingGenerationConsumer(
    IUnitOfWork unitOfWork,
    IEmbeddingService embeddingService,
    ILogger<EmbeddingGenerationConsumer> logger)
    : IConsumer<MaintenanceRecordCreatedEvent>
{
    public async Task Consume(ConsumeContext<MaintenanceRecordCreatedEvent> context)
    {
        var message = context.Message;
        var ct = context.CancellationToken;

        var record = await unitOfWork.MaintenanceRecords.GetByIdAsync(message.RecordId, ct);
        if (record is null)
        {
            logger.LogWarning(
                "MaintenanceRecord {RecordId} not found for embedding generation",
                message.RecordId);
            return;
        }

        if (record.NotesEmbedding is not null)
        {
            logger.LogInformation(
                "Embedding already exists for record {RecordId}, skipping",
                message.RecordId);
            return;
        }

        record.NotesEmbedding = await embeddingService.GenerateEmbeddingAsync(message.Notes, ct);
        await unitOfWork.MaintenanceRecords.UpdateAsync(record);
        await unitOfWork.SaveChangesAsync(ct);

        logger.LogInformation(
            "Embedding generated for MaintenanceRecord {RecordId}",
            message.RecordId);
    }
}