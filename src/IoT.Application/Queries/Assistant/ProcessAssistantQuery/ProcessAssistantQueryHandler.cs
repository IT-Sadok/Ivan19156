using IoT.Application.Queries.Assistant.ProcessAssistantQuery;
using IoT.Infrastructure.Services;
using IoT.Shared.Mediator;
using IoT.Interfaces.Services;
using IoT.Shared.Common;
using Microsoft.Extensions.Options;

public class ProcessAssistantQueryHandler(
    IAIAssistantService aiService,
    IoTContextBuilder contextBuilder,
    IOptions<AzureAIOptions> options) : IRequestHandler<ProcessAssistantQuery, Result<string>>
{
    public async Task<Result<string>> Handle(ProcessAssistantQuery request, CancellationToken ct)
    {
        string? context = null;

        if (options.Value.Mode == AIAssistantMode.ContextInjection)
            context = await contextBuilder.BuildAsync(ct);

        var answer = await aiService.ProcessQueryAsync(request.UserQuery, context, ct);
        return Result<string>.Success(answer);
    }
}