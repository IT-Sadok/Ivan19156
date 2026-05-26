namespace IoT.Interfaces.Services;

public interface IAIAssistantService
{
    Task<string> ProcessQueryAsync(string userQuery, string systemContext, CancellationToken ct = default);
}