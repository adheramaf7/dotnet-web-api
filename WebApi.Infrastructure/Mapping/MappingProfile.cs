using AutoMapper;
using WebApi.Application.DTOs.Response;
using WebApi.Infrastructure.Identity;

namespace WebApi.Infrastructure.Mapping
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            CreateMap<ApplicationUser, UserProfileResponse>();
        }
    }
}