using IoT.Application.Queries.Alerts.GetAlerts;
using IoT.Contracts.Alerts;
using IoT.Shared.Common;
using IoT.Shared.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IoT.Rest.Controllers;

[Authorize]
[Route("api/alerts")]
public class AlertsController : BaseController
{
    private readonly IMediator _mediator;

    public AlertsController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Guid? deviceId = null)
        => HandleResult(await _mediator.SendAsync<GetAlertsQuery, Result<IEnumerable<AlertResponse>>>(
            new GetAlertsQuery(deviceId)));
}