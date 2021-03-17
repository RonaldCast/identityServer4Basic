using AutoMapper;
using Identity_server.Data.DomainModel;
using Identity_server.DTO;

namespace Identity_server.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserSignUp, User>()
                .ForMember(u => u.UserName, opt
                    => opt.MapFrom(ur => ur.Email));
        }
    }
}