using TelemetryService.Domain.Abstractions;

namespace TelemetryService.Domain.Entities;

// TODO: Remove when Gateway handles API key validation
public class DeviceApiKey : BaseEntity
{
    public Guid DeviceId { get; set; }
    public string Prefix { get; set; } = string.Empty;
    public string KeyHash { get; set; } = string.Empty;
    public DateTime? LastUsedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
}