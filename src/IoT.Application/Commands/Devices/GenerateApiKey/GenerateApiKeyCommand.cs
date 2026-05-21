using IoT.Interfaces.Mediator;
using IoT.Shared.Common;

namespace IoT.Application.Commands.Devices.GenerateApiKey;

public record GenerateApiKeyCommand(Guid DeviceId) : IRequest<Result<string>>;