// IoT.Infrastructure/Services/AzureAIAssistantService.cs

using Azure;
using Azure.AI.OpenAI;
using IoT.Interfaces.Services;
using Microsoft.Extensions.Options;
using OpenAI.Chat;

namespace IoT.Infrastructure.Services;

public class AzureAIAssistantService(
    AzureOpenAIClient client,
    IOptions<AzureAIOptions> options) : IAIAssistantService
{
    public async Task<string> ProcessQueryAsync(
        string userQuery,
        string systemContext,
        CancellationToken ct = default)
    {
        var chatClient = client.GetChatClient(options.Value.DeploymentName);

        var messages = new List<ChatMessage>
        {
            new SystemChatMessage($"{options.Value.SystemPrompt}\n\n{systemContext}"),
            new UserChatMessage(userQuery)
        };

        var response = await chatClient.CompleteChatAsync(messages, cancellationToken: ct);
        return response.Value.Content[0].Text;
    }
}