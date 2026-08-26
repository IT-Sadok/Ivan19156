using DeviceService.Application.Commands.AcknowledgeAlert;
using DeviceService.Application.Queries.GetAlerts;
using IoT.Contracts.Alerts;
using IoT.Shared.Common;
using IoT.Shared.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DeviceService.Interfaces.Services;

namespace DeviceService.Rest.Controllers;

[Route("api/alerts")]
[Authorize]
public class AlertsController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IUserContext _userContext;

    public AlertsController(IMediator mediator,  IUserContext userContext)
    {
        _mediator = mediator;
        _userContext = userContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Guid? deviceId)
    {
        var query = new GetAlertsQuery(deviceId);
        return HandleResult(await _mediator.SendAsync<GetAlertsQuery, Result<IEnumerable<AlertResponse>>>(query));
    }

    [HttpPost("{id:guid}/acknowledge")]
    public async Task<IActionResult> Acknowledge(Guid id, [FromBody] AcknowledgeAlertRequest request)
    {
        var command = new AcknowledgeAlertCommand(id, _userContext.UserId, request.Note);
        return HandleResult(await _mediator.SendAsync<AcknowledgeAlertCommand, Result<bool>>(command));
    }
}

public record AcknowledgeAlertRequest(string? Note);
