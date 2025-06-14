using AutoMapper;
using WebApi.Application.DTOs.Request.Contact;
using WebApi.Application.DTOs.Response;
using WebApi.Domain.Entities;
using WebApi.Infrastructure.Identity;

namespace WebApi.Infrastructure.Mapping
{
    public class ContactMappingProfile : Profile
    {
        public ContactMappingProfile()
        {
            CreateMap<SaveContactRequest, Contact>();

            CreateMap<Contact, ContactResponse>();
        }
    }
}