using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Application.DTOs.Request.Contact;
using WebApi.Application.DTOs.Response;

namespace WebApi.Application.Interfaces.Service
{
    public interface IContactService
    {
        Task<IList<ContactResponse>> GetAllAsync(string userId);
        Task<ContactResponse> GetByIdAsync(int id);
        Task<ContactResponse> CreateAsync(SaveContactRequest request);
        Task<ContactResponse> UpdateAsync(int id, SaveContactRequest request);
        Task<bool> DeleteAsync(int id);
    }
}