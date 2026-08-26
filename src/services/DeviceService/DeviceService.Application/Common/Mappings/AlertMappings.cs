using DeviceService.Domain.Entities;
using IoT.Contracts.Alerts;
using IoT.Domain.Enums;

namespace DeviceService.Application.Common.Mappings;

public static class AlertMappings
{
    public static AlertResponse ToDto(this Alert alert) => new(
        alert.Id,
        alert.DeviceId,
        alert.RuleId,
        alert.Rule?.Name ?? string.Empty,
        MapAlertStatus(alert.Status),
        alert.CreatedAt,
        alert.Status == DeviceService.Domain.Enums.AlertStatus.Resolved ? alert.UpdatedAt : null);

    private static AlertStatus MapAlertStatus(DeviceService.Domain.Enums.AlertStatus status) => status switch
    {
        DeviceService.Domain.Enums.AlertStatus.New => AlertStatus.New,
        DeviceService.Domain.Enums.AlertStatus.Acknowledged => AlertStatus.Acknowledged,
        DeviceService.Domain.Enums.AlertStatus.Resolved => AlertStatus.Resolved,
        _ => AlertStatus.New
    };
}
