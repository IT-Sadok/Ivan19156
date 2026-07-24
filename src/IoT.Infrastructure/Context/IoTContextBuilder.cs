// IoT.Infrastructure/Context/IoTContextBuilder.cs

using System.Text;
using IoT.Interfaces.Repositories;

public class IoTContextBuilder(
    IDeviceRepository deviceRepository,
    IAlertRepository alertRepository,
    ITelemetryRepository telemetryRepository)
{
    public async Task<string> BuildAsync(CancellationToken ct = default)
    {
        var now = DateTimeOffset.UtcNow;

        var sb = new StringBuilder();
        sb.AppendLine($"[IoT System Snapshot — {now:yyyy-MM-dd HH:mm} UTC]");
        sb.AppendLine();

        await AppendDevicesSection(sb, now, ct);
        await AppendAlertsSection(sb, now, ct);
        await AppendTelemetrySection(sb, now, ct);

        return sb.ToString();
    }

    private async Task AppendDevicesSection(StringBuilder sb, DateTimeOffset now, CancellationToken ct)
    {
        var offlineThreshold = now.AddMinutes(-15);
        var devices = (await deviceRepository.GetAllWithStatusAsync(ct)).ToList();

        var online = devices.Where(d => d.LastSeen != null && d.LastSeen > offlineThreshold).ToList();
        var offline = devices.Where(d => d.LastSeen == null || d.LastSeen <= offlineThreshold).ToList();

        sb.AppendLine($"DEVICES (total: {devices.Count}):");
        sb.AppendLine($"  Online: {online.Count}");
        sb.AppendLine($"  Offline: {offline.Count}");

        foreach (var d in devices)
        {
            var status = online.Contains(d) ? "ONLINE" : "OFFLINE";
            var lastSeen = d.LastSeen.HasValue
                ? $"last seen {(now - d.LastSeen.Value).TotalMinutes:F0}m ago"
                : "never seen";
            sb.AppendLine($"  - {d.Name} [{d.Type}] {status} ({lastSeen})");
        }

        sb.AppendLine();
    }

    private async Task AppendAlertsSection(StringBuilder sb, DateTimeOffset now, CancellationToken ct)
    {
        var activeAlerts = (await alertRepository.GetActiveAsync(ct)).ToList();

        sb.AppendLine($"ACTIVE ALERTS ({activeAlerts.Count}):");

        if (activeAlerts.Count == 0)
        {
            sb.AppendLine("  No active alerts.");
        }
        else
        {
            foreach (var a in activeAlerts)
                sb.AppendLine($"  - Device '{a.Device.Name}', Rule: '{a.Rule.Name}', triggered {(now - a.TriggeredAt).TotalMinutes:F0}m ago");
        }

        sb.AppendLine();
    }

    private async Task AppendTelemetrySection(StringBuilder sb, DateTimeOffset now, CancellationToken ct)
    {
        var recentTelemetry = (await telemetryRepository.GetRecentSummaryAsync(now.AddHours(-1), ct)).ToList();
        sb.AppendLine("TELEMETRY (last 1h):");

        if (!recentTelemetry.Any())
            sb.AppendLine("  No telemetry in the last hour.");
        else
            foreach (var t in recentTelemetry)
                sb.AppendLine($"  - Device {t.DeviceId}: {t.Count} records, last at {t.LastAt:HH:mm}");
    }
}