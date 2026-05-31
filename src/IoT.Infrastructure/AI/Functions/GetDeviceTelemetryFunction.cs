using System.Text.Json;
using IoT.Interfaces.Repositories;

namespace IoT.Infrastructure.AI.Functions;

public class GetDeviceTelemetryFunction(ITelemetryRepository telemetryRepository) : AIFunctionBase
{
    public override string Name => "get_device_telemetry";
    public override string Description => "Returns recent telemetry records for a specific device";
    public override BinaryData Parameters => BinaryData.FromString("""
                                                                   {
                                                                       "type": "object",
                                                                       "properties": {
                                                                           "device_id": {
                                                                               "type": "string",
                                                                               "description": "The UUID of the device"
                                                                           }
                                                                       },
                                                                       "required": ["device_id"]
                                                                   }
                                                                   """);

    public override async Task<string> ExecuteAsync(string arguments, CancellationToken ct = default)
    {
        using var doc = JsonDocument.Parse(arguments);
        var deviceId = Guid.Parse(doc.RootElement.GetProperty("device_id").GetString()!);

        var records = await telemetryRepository.GetByDeviceIdAsync(deviceId, ct);
        var result = records.Take(10).Select(t => new
        {
            payload = t.Payload,
            receivedAt = t.ReceivedAt
        });
        return SerializeResult(result);
    }
}