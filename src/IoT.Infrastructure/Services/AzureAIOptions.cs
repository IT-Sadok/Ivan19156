namespace IoT.Infrastructure.Services;

public enum AIAssistantMode
{
    ContextInjection,
    FunctionCalling
}
public class AzureAIOptions
{
    public const string SectionName = "AzureAI";
    public string Endpoint { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public string DeploymentName { get; set; } = string.Empty;
    public string SystemPrompt { get; set; } = string.Empty;
    public AIAssistantMode Mode { get; set; } = AIAssistantMode.FunctionCalling;
}