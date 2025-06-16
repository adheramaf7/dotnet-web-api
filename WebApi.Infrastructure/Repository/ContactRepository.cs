using WebApi.Application.Interfaces.Repository;
using WebApi.Domain.Entities;
using WebApi.Infrastructure.Data;

namespace WebApi.Infrastructure.Repository
{
    public class ContactRepository(AppDbContext dbContext) : Repository<Contact>(dbContext), IContactRepository
    {
    }
}