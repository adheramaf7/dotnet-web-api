using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Application.Common;
using WebApi.Application.DTOs.Request.Contact;
using WebApi.Application.DTOs.Response;
using WebApi.Domain.Entities;

namespace WebApi.Application.Interfaces.Service
{
    public interface IContactService
    {
        Task<PaginatedResponse<ContactResponse>> GetAllAsync(string userId, int pageNumber = 1, int pageSize = 10);
        Task<ContactResponse> GetByIdAsync(string userId, int id);
        Task<ContactResponse> CreateAsync(string userId, SaveContactRequest request);
        Task<ContactResponse> UpdateAsync(int id, string userId, SaveContactRequest request);
        Task<bool> DeleteAsync(int id, string userId);
    }
}