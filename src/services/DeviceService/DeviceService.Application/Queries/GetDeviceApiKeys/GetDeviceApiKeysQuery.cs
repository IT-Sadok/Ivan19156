using IoT.Shared.Common;
using IoT.Shared.Mediator;

namespace DeviceService.Application.Queries.GetDeviceApiKeys;

public record GetDeviceApiKeysQuery(Guid DeviceId) : IRequest<Result<IEnumerable<ApiKeyDto>>>;
