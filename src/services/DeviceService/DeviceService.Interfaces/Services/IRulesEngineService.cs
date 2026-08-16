namespace DeviceService.Interfaces.Services;

public interface IRulesEngineService
{
    Task EvaluateAsync(Guid deviceId, string metricKey, double value, CancellationToken ct = default);
}
