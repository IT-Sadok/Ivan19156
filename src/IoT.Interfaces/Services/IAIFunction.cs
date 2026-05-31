namespace IoT.Interfaces.Services;

public interface IAIFunction
{
    string Name { get; }
    string Description { get; }
    BinaryData Parameters { get; }
    Task<string> ExecuteAsync(string arguments, CancellationToken ct = default);
}