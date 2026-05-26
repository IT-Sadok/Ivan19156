using AutoMapper;
using IoT.Application.Commands.Identity.Login;
using IoT.Application.Commands.Identity.Register;
using IoT.Contracts.Identity;

namespace IoT.Application.Common.Mappings;

public class IdentityMappingProfile : Profile
{
    public IdentityMappingProfile()
    {
        CreateMap<RegisterRequest, RegisterCommand>();
        CreateMap<LoginRequest, LoginCommand>();
    }
}
