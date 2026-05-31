using Azure.AI.OpenAI;
using IoT.Infrastructure.Services;
using IoT.Interfaces.Services;
using Microsoft.Extensions.Options;
using OpenAI.Chat;

public class AzureAIAssistantService(
    AzureOpenAIClient client,
    IOptions<AzureAIOptions> options,
    IEnumerable<IAIFunction> functions) : IAIAssistantService
{
    public async Task<string> ProcessQueryAsync(
        string userQuery,
        string? systemContext = null,
        CancellationToken ct = default)
    {
        return options.Value.Mode switch
        {
            AIAssistantMode.ContextInjection => await ProcessWithContextInjectionAsync(userQuery, systemContext, ct),
            AIAssistantMode.FunctionCalling => await ProcessWithFunctionCallingAsync(userQuery, ct),
            _ => throw new InvalidOperationException("Unknown AI assistant mode")
        };
    }

    private async Task<string> ProcessWithContextInjectionAsync(
        string userQuery,
        string? systemContext,
        CancellationToken ct)
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

    private async Task<string> ProcessWithFunctionCallingAsync(
        string userQuery,
        CancellationToken ct)
    {
        var chatClient = client.GetChatClient(options.Value.DeploymentName);

        var chatOptions = new ChatCompletionOptions();
        foreach (var f in functions)
            chatOptions.Tools.Add(ChatTool.CreateFunctionTool(f.Name, f.Description, f.Parameters));

        var messages = new List<ChatMessage>
        {
            new SystemChatMessage(options.Value.SystemPrompt),
            new UserChatMessage(userQuery)
        };

        while (true)
        {
            var response = await chatClient.CompleteChatAsync(messages, chatOptions, ct);

            if (response.Value.FinishReason == ChatFinishReason.Stop)
                return response.Value.Content[0].Text;

            if (response.Value.FinishReason == ChatFinishReason.ToolCalls)
            {
                messages.Add(new AssistantChatMessage(response.Value.ToolCalls));

                foreach (var toolCall in response.Value.ToolCalls)
                {
                    var function = functions.FirstOrDefault(f => f.Name == toolCall.FunctionName);
                    var result = function != null
                        ? await function.ExecuteAsync(toolCall.FunctionArguments.ToString(), ct)
                        : "Function not found";

                    messages.Add(new ToolChatMessage(toolCall.Id, result));
                }
            }
        }
    }
}