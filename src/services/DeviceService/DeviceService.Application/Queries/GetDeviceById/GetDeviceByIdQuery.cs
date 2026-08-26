using IoT.Contracts.Devices;
using IoT.Shared.Common;
using IoT.Shared.Mediator;

namespace DeviceService.Application.Queries.GetDeviceById;

public record GetDeviceByIdQuery(Guid Id) : IRequest<Result<DeviceResponse>>;
