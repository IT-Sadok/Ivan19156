namespace MaintenanceService.Domain.ValueObjects;

public record DeviceId(Guid Value)
{
    public static DeviceId From(Guid value) => new(value);
}