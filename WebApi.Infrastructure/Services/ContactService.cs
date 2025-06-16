using AutoMapper;
using WebApi.Application.Common;
using WebApi.Application.DTOs.Request.Contact;
using WebApi.Application.DTOs.Response;
using WebApi.Application.Exceptions;
using WebApi.Application.Interfaces.Repository;
using WebApi.Application.Interfaces.Service;
using WebApi.Domain.Entities;

namespace WebApi.Infrastructure.Services
{
    public class ContactService(IContactRepository contactRepository, IMapper mapper) : IContactService
    {
        private readonly IContactRepository contactRepository = contactRepository;
        private readonly IMapper mapper = mapper;

        public async Task<PaginatedResponse<ContactResponse>> GetAllAsync(string userId, int pageNumber = 1, int pageSize = 10)
        {
            var (items, pageCount) = await contactRepository.GetPagedAsync(
                filter: (x) => x.UserId == userId,
                pageNumber: 1,
                pageSize: 10
            );

            return new PaginatedResponse<ContactResponse>
            {
                Items = mapper.Map<IList<ContactResponse>>(items),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = items.Count(),
            };
        }

        public async Task<ContactResponse> GetByIdAsync(string userId, int id)
        {
            var contact = await contactRepository.GetOneAsync(
                filter: x => x.Id == id && x.UserId == userId
            ) ?? throw new NotFoundException("Contact");

            var contactResponse = mapper.Map<ContactResponse>(contact);

            return contactResponse;
        }

        public async Task<ContactResponse> CreateAsync(string userId, SaveContactRequest request)
        {
            var contact = mapper.Map<Contact>(request);
            contact.UserId = userId;

            await contactRepository.CreateAsync(contact);

            return mapper.Map<ContactResponse>(contact);
        }

        public async Task<ContactResponse> UpdateAsync(int id, string userId, SaveContactRequest request)
        {
            var contact = await contactRepository.GetByIdAsync(id) ?? throw new NotFoundException("Contact");

            if (contact.UserId != userId)
            {
                throw new AccessForbiddenException("You don't have permission to edit this contact.");
            }

            mapper.Map(request, contact);

            await contactRepository.UpdateAsync(contact);

            return mapper.Map<ContactResponse>(contact);
        }

        public async Task<bool> DeleteAsync(int id, string userId)
        {
            var contact = await contactRepository.GetByIdAsync(id) ?? throw new NotFoundException("Contact");

            if (contact.UserId != userId)
            {
                throw new AccessForbiddenException("You don't have permission to delete this contact.");
            }

            await contactRepository.DeleteAsync(contact);

            return true;
        }

    }
}