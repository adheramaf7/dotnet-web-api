using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Application.DTOs.Request.Contact;
using WebApi.Application.DTOs.Response;
using WebApi.Application.Interfaces.Service;

namespace WebApi.Infrastructure.Services
{
    public class ContactService : IContactService
    {
        public Task<ContactResponse> CreateAsync(SaveContactRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IList<ContactResponse>> GetAllAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<ContactResponse> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ContactResponse> UpdateAsync(int id, SaveContactRequest request)
        {
            throw new NotImplementedException();
        }
    }
}