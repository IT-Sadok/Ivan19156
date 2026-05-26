using AutoMapper;
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
    private readonly IMapper _mapper;

    public CreateRuleCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<RuleResponse>> Handle(
        CreateRuleCommand request,
        CancellationToken ct = default)
    {
        var rule = _mapper.Map<Rule>(request);

        await _unitOfWork.Rules.AddAsync(rule, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result<RuleResponse>.Success(_mapper.Map<RuleResponse>(rule));
    }
}