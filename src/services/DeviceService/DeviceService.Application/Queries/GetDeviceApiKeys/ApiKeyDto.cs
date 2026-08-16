namespace DeviceService.Application.Queries.GetDeviceApiKeys;

public record ApiKeyDto(Guid Id, string Prefix, bool IsActive, DateTime? ExpiresAt);
