using Gatam.Application.CQRS;
using AutoMapper;
using Gatam.Domain;
namespace Gatam.Application
{
    public class Mapping:Profile
    {
        public Mapping()
        {
            CreateMap<ApplicationUser, UserDTO>()
                .ForMember(dest => dest.Nickname, opt => opt.MapFrom(src => src.UserName));
            CreateMap<UserDTO, ApplicationUser>() .ForMember(dest => dest.Id, opt => opt.Ignore());

        }
    }
}
