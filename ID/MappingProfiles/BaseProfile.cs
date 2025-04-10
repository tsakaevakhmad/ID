using AutoMapper;
using ID.Commands.Admin;
using ID.Domain.Entity;

namespace ID.MappingProfiles
{
    public class BaseProfile : Profile
    {
        public BaseProfile() 
        {
            CreateMap<RegisterCommand, User>();
        }
    }
}
