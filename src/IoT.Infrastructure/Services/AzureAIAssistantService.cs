// IoT.Infrastructure/Services/AzureAIAssistantService.cs

using Azure;
using Azure.AI.OpenAI;
using IoT.Interfaces.Services;
using Microsoft.Extensions.Options;
using OpenAI.Chat;

namespace IoT.Infrastructure.Services;

public class AzureAIAssistantService(IOptions<AzureAIOptions> options) : IAIAssistantService
{
    private const string SystemPrompt = """
                                        You are an IoT system monitoring assistant.
                                        Answer questions based ONLY on the provided system snapshot data.
                                        Be concise and precise. If data is not in the snapshot, say so.
                                        Always respond in the same language the user used.
                                        """;

    public async Task<string> ProcessQueryAsync(
        string userQuery,
        string systemContext,
        CancellationToken ct = default)
    {
        var client = new AzureOpenAIClient(
            new Uri(options.Value.Endpoint),
            new AzureKeyCredential(options.Value.ApiKey));

        var chatClient = client.GetChatClient(options.Value.DeploymentName);

        var messages = new List<ChatMessage>
        {
            new SystemChatMessage($"{SystemPrompt}\n\n{systemContext}"),
            new UserChatMessage(userQuery)
        };

        var response = await chatClient.CompleteChatAsync(messages, cancellationToken: ct);

        return response.Value.Content[0].Text;
    }
}