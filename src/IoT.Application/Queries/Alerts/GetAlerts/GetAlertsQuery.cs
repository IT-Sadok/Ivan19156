using IoT.Contracts.Alerts;
using IoT.Shared.Mediator;
using IoT.Shared.Common;

namespace IoT.Application.Queries.Alerts.GetAlerts;

public record GetAlertsQuery(Guid? DeviceId = null) : IRequest<Result<IEnumerable<AlertResponse>>>;