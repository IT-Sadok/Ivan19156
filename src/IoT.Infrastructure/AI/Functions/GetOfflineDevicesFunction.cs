using IoT.Interfaces.Repositories;

namespace IoT.Infrastructure.AI.Functions;

public class GetOfflineDevicesFunction(IDeviceRepository deviceRepository) : AIFunctionBase
{
    public override string Name => "get_offline_devices";
    public override string Description => "Returns list of devices that are currently offline (no telemetry in last 15 minutes)";
    public override BinaryData Parameters => BinaryData.FromString("""
                                                                   {
                                                                       "type": "object",
                                                                       "properties": {},
                                                                       "required": []
                                                                   }
                                                                   """);

    public override async Task<string> ExecuteAsync(string arguments, CancellationToken ct = default)
    {
        var devices = await deviceRepository.GetOfflineDevicesAsync(ct);
        var result = devices.Select(d => new
        {
            id = d.Id,
            name = d.Name,
            type = d.Type.ToString(),
            lastSeen = d.LastSeen
        });
        return SerializeResult(result);
    }
}