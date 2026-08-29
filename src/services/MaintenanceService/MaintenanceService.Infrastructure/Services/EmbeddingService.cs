using Azure.AI.OpenAI;
using MaintenanceService.Infrastructure.Options;
using MaintenanceService.Interfaces.Services;
using Microsoft.Extensions.Options;

namespace MaintenanceService.Infrastructure.Services;

public class EmbeddingService(
    AzureOpenAIClient client,
    IOptions<AzureAIOptions> options) : IEmbeddingService
{
    public async Task<float[]> GenerateEmbeddingAsync(string text, CancellationToken ct = default)
    {
        var embeddingClient = client.GetEmbeddingClient(options.Value.EmbeddingDeploymentName);
        var response = await embeddingClient.GenerateEmbeddingAsync(text, cancellationToken: ct);
        return response.Value.ToFloats().ToArray();
    }
}