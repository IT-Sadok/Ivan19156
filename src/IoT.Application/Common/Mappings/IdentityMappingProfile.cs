using AutoMapper;
using IoT.Application.Commands.Identity.Login;
using IoT.Application.Commands.Identity.Register;
using IoT.Contracts.Identity;

namespace IoT.Application.Common.Mappings;

public class IdentityMappingProfile : Profile
{
    public IdentityMappingProfile()
    {
        CreateMap<RegisterRequest, RegisterCommand>()
            .ConstructUsing(src => new RegisterCommand(
                src.UserName,
                src.Email,
                src.Password,
                src.Role));;
        CreateMap<LoginRequest, LoginCommand>();
    }
}
