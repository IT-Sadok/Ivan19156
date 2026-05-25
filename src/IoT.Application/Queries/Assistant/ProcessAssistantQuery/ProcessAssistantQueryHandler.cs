using IoT.Interfaces.Mediator;
using IoT.Interfaces.Services;
using IoT.Shared.Common;

namespace IoT.Application.Queries.Assistant.ProcessAssistantQuery;

public class ProcessAssistantQueryHandler(
    IAIAssistantService aiService,
    IoTContextBuilder contextBuilder) : IRequestHandler<ProcessAssistantQuery, Result<string>>
{
    public async Task<Result<string>> Handle(ProcessAssistantQuery request, CancellationToken ct)
    {
        var context = await contextBuilder.BuildAsync(ct);
        var answer = await aiService.ProcessQueryAsync(request.UserQuery, context, ct);
        return Result<string>.Success(answer);
    }
}