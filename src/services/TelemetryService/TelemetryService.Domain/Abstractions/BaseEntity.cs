namespace TelemetryService.Domain.Abstractions;

public abstract class BaseEntity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
}