// IoT.Infrastructure/AI/Functions/GetSystemSummaryFunction.cs

using IoT.Interfaces.Repositories;

namespace IoT.Infrastructure.AI.Functions;

public class GetSystemSummaryFunction(
    IDeviceRepository deviceRepository,
    IAlertRepository alertRepository) : AIFunctionBase
{
    public override string Name => "get_system_summary";
    public override string Description => "Returns a high-level summary of the IoT system: total devices, online/offline counts, active alerts count";
    public override BinaryData Parameters => BinaryData.FromString("""
                                                                   {
                                                                       "type": "object",
                                                                       "properties": {},
                                                                       "required": []
                                                                   }
                                                                   """);

    public override async Task<string> ExecuteAsync(string arguments, CancellationToken ct = default)
    {
        var now = DateTimeOffset.UtcNow;
        var offlineThreshold = now.AddMinutes(-15);

        var devices = (await deviceRepository.GetAllAsync(ct)).ToList();
        var activeAlerts = (await alertRepository.GetActiveAsync(ct)).ToList();

        var result = new
        {
            totalDevices = devices.Count,
            onlineDevices = devices.Count(d => d.LastSeen != null && d.LastSeen > offlineThreshold),
            offlineDevices = devices.Count(d => d.LastSeen == null || d.LastSeen <= offlineThreshold),
            activeAlertsCount = activeAlerts.Count
        };

        return SerializeResult(result);
    }
}