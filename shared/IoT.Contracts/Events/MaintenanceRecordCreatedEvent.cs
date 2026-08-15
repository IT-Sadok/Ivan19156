namespace IoT.Contracts.Events;

public record MaintenanceRecordCreatedEvent
{
    public Guid RecordId { get; init; }
    public string Notes { get; init; } = string.Empty;
}