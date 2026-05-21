using IoT.Contracts.Alerts;
using IoT.Domain.Entities;
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
        var rule = new Rule
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            DeviceId = request.DeviceId,
            DeviceType = request.DeviceType,
            Field = request.Field,
            Operator = request.Operator,
            Value = request.Value,
            Action = request.Action,
            IsActive = true,
            CreatedById = request.CreatedById
        };

        await _unitOfWork.Rules.AddAsync(rule, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result<RuleResponse>.Success(new RuleResponse(
            rule.Id,
            rule.Name,
            rule.DeviceId,
            rule.DeviceType,
            rule.Field,
            rule.Operator,
            rule.Value,
            rule.Action,
            rule.IsActive,
            rule.CreatedAt
        ));
    }
}