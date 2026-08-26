using DeviceService.Domain.Enums;
using IoT.Contracts.Devices;
using IoT.Shared.Common;
using IoT.Shared.Mediator;

namespace DeviceService.Application.Queries.GetDevices;

public record GetDevicesQuery(
    int Page = 1,
    int PageSize = 20,
    DeviceType? Type = null) : IRequest<Result<PagedResult<DeviceResponse>>>;
