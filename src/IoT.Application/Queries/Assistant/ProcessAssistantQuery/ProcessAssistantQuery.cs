using IoT.Shared.Mediator;
using IoT.Shared.Common;

namespace IoT.Application.Queries.Assistant.ProcessAssistantQuery;

public record ProcessAssistantQuery(string UserQuery) : IRequest<Result<string>>;