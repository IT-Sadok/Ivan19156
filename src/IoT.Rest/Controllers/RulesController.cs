using IoT.Application.Queries.Alerts.GetRules;
using IoT.Contracts.Alerts;
using IoT.Domain.Constants;
using IoT.Shared.Common;
using IoT.Shared.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IoT.Rest.Controllers;

[Authorize]
[Route("api/rules")]
public class RulesController : BaseController
{
    private readonly IMediator _mediator;

    public RulesController(IMediator mediator)
        => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => HandleResult(await _mediator.SendAsync<GetRulesQuery, Result<IEnumerable<RuleResponse>>>(
            new GetRulesQuery()));

    
}