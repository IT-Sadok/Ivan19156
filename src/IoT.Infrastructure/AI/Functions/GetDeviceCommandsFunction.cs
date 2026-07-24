using System.Text.Json;
using IoT.Interfaces.Repositories;

namespace IoT.Infrastructure.AI.Functions;

public class GetDeviceCommandsFunction(IDeviceCommandRepository deviceCommandRepository) : AIFunctionBase
{
    public override string Name => "get_device_commands";
    public override string Description => "Returns pending commands for a specific device";
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

        var commands = await deviceCommandRepository.GetCreatedByDeviceAsync(deviceId, ct);
        var result = commands.Select(c => new
        {
            id = c.Id,
            commandType = c.CommandType.Slug,
            status = c.Status.ToString(),
            priority = c.Priority,
            createdAt = c.CreatedAt
        });
        return SerializeResult(result);
    }
}