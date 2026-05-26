using AutoMapper;
using IoT.Application.Commands.Alerts.CreateRule;
using IoT.Contracts.Alerts;
using IoT.Domain.Entities;

public class RuleMappingProfile : Profile
{
    public RuleMappingProfile()
    {
        CreateMap<CreateRuleCommand, Rule>()
            .ForMember(d => d.Id, o => o.MapFrom(_ => Guid.NewGuid()))
            .ForMember(d => d.IsActive, o => o.MapFrom(_ => true));
            
        CreateMap<Rule, RuleResponse>();
    }
}