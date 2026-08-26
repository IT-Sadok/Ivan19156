using IoT.Contracts.Alerts;
using IoT.Shared.Common;
using IoT.Shared.Mediator;

namespace DeviceService.Application.Queries.GetAlerts;

public record GetAlertsQuery(Guid? DeviceId = null) : IRequest<Result<IEnumerable<AlertResponse>>>;
