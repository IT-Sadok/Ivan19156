using IoT.Shared.Common;
using IoT.Shared.Mediator;

namespace DeviceService.Application.Commands.GenerateApiKey;

public record GenerateApiKeyCommand(Guid DeviceId) : IRequest<Result<string>>;
