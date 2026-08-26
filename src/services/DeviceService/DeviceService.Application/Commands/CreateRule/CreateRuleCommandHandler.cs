using DeviceService.Domain.Entities;
using DeviceService.Interfaces;
using IoT.Shared.Common;
using IoT.Shared.Mediator;

namespace DeviceService.Application.Commands.CreateRule;

public class CreateRuleCommandHandler : IRequestHandler<CreateRuleCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateRuleCommandHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> ExecuteAsync(CreateRuleCommand request, CancellationToken ct = default)
    {
        if (request.DeviceId is null && request.DeviceType is null)
            return Result<Guid>.Failure("Either DeviceId or DeviceType must be specified.");

        if (request.DeviceId is not null && request.DeviceType is not null)
            return Result<Guid>.Failure("Only one of DeviceId or DeviceType may be specified, not both.");

        var rule = new Rule
        {
            Name = request.Name,
            MetricKey = request.MetricKey,
            Operator = request.Operator,
            Threshold = request.Threshold,
            Action = request.Action,
            DeviceId = request.DeviceId,
            DeviceType = request.DeviceType,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Rules.AddAsync(rule, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return Result<Guid>.Success(rule.Id);
    }
}
