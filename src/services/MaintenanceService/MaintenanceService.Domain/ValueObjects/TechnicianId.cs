namespace MaintenanceService.Domain.ValueObjects;

public record TechnicianId(Guid Value)
{
    public static TechnicianId From(Guid value) => new(value);
}