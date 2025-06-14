using AutoMapper;
using WebApi.Application.DTOs.Response;
using WebApi.Infrastructure.Identity;

namespace WebApi.Infrastructure.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<ApplicationUser, UserProfileResponse>();
        }
    }
}