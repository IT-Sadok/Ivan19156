using IoT.Application.Commands.Telemetry.ProcessTelemetry;
using IoT.Contracts.Telemetry;

namespace IoT.Application.Common.Mappings;

public static class TelemetryMappingExtensions
{
    public static ProcessTelemetryCommand ToCommand(this ProcessTelemetryRequest request, Guid deviceId)
        => new(deviceId, request.MessageId, request.Payload);
}
