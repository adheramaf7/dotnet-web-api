using System.Linq.Expressions;
using WebApi.Domain.Common;

namespace WebApi.Application.Common
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<IList<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, bool tracked = false, params string[] includeProperties);
        Task<(IList<T> Items, int TotalCount)> GetPagedAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            int pageNumber = 1,
            int pageSize = 10,
            bool tracked = false,
            params string[] includeProperties
        );
        Task<T?> GetOneAsync(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, bool tracked = false, params string[] includeProperties);
        Task<int> CountDataAsync(Expression<Func<T, bool>>? filter = null);
        Task<T?> GetByIdAsync(int id, bool tracked = false);
        Task<T?> GetByIdAsync(Guid id, bool tracked = false);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task DeleteManyAsync(List<T> entities);
        Task SaveChangesAsync();
        Task BeginTransaction();
        Task CommitTransaction();
        Task RollbackTransaction();
    }
}