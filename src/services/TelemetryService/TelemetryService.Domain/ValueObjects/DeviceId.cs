namespace TelemetryService.Domain.ValueObjects;

public record DeviceId(Guid Value)
{
    public static DeviceId New() => new(Guid.NewGuid());
    public static DeviceId From(Guid value) => new(value);
}