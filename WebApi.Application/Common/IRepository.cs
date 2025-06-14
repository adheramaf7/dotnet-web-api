using System.Linq.Expressions;
using WebApi.Domain.Common;

namespace WebApi.Application.Common
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<IList<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, List<string>? includeProperties = null, bool tracked = false);
        Task<int> CountDataAsync(Expression<Func<T, bool>>? filter = null);
        Task<T> GetByIdAsync(int id);
        Task<T> GetByGuidAsync(Guid guid);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task DeleteManyAsync(List<T> entity);
    }
}