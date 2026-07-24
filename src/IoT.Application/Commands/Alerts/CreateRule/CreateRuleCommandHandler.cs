using IoT.Application.Common.Mappings;
using IoT.Contracts.Alerts;
using IoT.Interfaces;
using IoT.Interfaces.Mediator;
using IoT.Shared.Common;

namespace IoT.Application.Commands.Alerts.CreateRule;

public class CreateRuleCommandHandler
    : IRequestHandler<CreateRuleCommand, Result<RuleResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateRuleCommandHandler(IUnitOfWork unitOfWork)
        => _unitOfWork = unitOfWork;

    public async Task<Result<RuleResponse>> Handle(
        CreateRuleCommand request,
        CancellationToken ct = default)
    {
        var rule = request.ToEntity();

        await _unitOfWork.Rules.AddAsync(rule, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result<RuleResponse>.Success(rule.ToResponse());
    }
}
