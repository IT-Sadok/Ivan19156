namespace MaintenanceService.Infrastructure.Options;

public class AzureAIOptions
{
    public const string SectionName = "AzureAI";
    public string Endpoint { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string EmbeddingDeploymentName { get; set; } = string.Empty;
}