using System.Text.Json;
using IoT.Interfaces.Services;
namespace IoT.Infrastructure.AI;

public abstract class AIFunctionBase : IAIFunction
{
    public abstract string Name { get; }
    public abstract string Description { get; }
    public abstract BinaryData Parameters { get; }
    public abstract Task<string> ExecuteAsync(string arguments, CancellationToken ct = default);

    protected static string SerializeResult<T>(T result)
        => JsonSerializer.Serialize(result, new JsonSerializerOptions
        {
            WriteIndented = false,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
}