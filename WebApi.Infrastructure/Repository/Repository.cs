using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;
using WebApi.Application.Common;
using WebApi.Domain.Common;
using WebApi.Infrastructure.Data;

namespace WebApi.Infrastructure.Repository
{
    public abstract class Repository<T>(AppDbContext dbContext) : IRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext _dbContext = dbContext;
        private readonly DbSet<T> _entities = dbContext.Set<T>();
        private IDbContextTransaction? _transaction;

        public async Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            bool tracked = false,
            params string[] includeProperties)
        {
            IQueryable<T> query = _entities;

            if (!tracked)
                query = query.AsNoTracking();

            if (filter is not null)
                query = query.Where(filter);

            if (orderBy is not null)
                query = orderBy(query);

            foreach (var includeProp in includeProperties)
                query = query.Include(includeProp);

            return await query.ToListAsync();
        }

        public async Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, int pageNumber = 1, int pageSize = 10, bool tracked = false, params string[] includeProperties)
        {
            IQueryable<T> query = _entities;

            if (!tracked)
                query = query.AsNoTracking();

            if (filter is not null)
                query = query.Where(filter);

            foreach (var includeProp in includeProperties)
                query = query.Include(includeProp);

            int totalCount = await query.CountAsync();

            if (orderBy is not null)
                query = orderBy(query);

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<T?> GetOneAsync(
            Expression<Func<T, bool>> filter,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            bool tracked = false,
            params string[] includeProperties)
        {
            IQueryable<T> query = _entities;

            if (!tracked)
                query = query.AsNoTracking();

            query = query.Where(filter);

            if (orderBy is not null)
                query = orderBy(query);

            foreach (var includeProp in includeProperties)
                query = query.Include(includeProp);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<int> CountDataAsync(Expression<Func<T, bool>>? filter = null)
        {
            return filter is null
                ? await _entities.CountAsync()
                : await _entities.CountAsync(filter);
        }

        public async Task<T?> GetByIdAsync(Guid id, bool tracked = false)
        {
            if (tracked)
                return await _entities.FindAsync(id);

            // Workaround for no-tracking with FindAsync
            var entity = await _entities.FindAsync(id);
            if (entity is not null)
                _dbContext.Entry(entity).State = EntityState.Detached;

            return entity;
        }

        public async Task<T?> GetByIdAsync(int id, bool tracked = false)
        {
            if (tracked)
                return await _entities.FindAsync(id);

            var entity = await _entities.FindAsync(id);
            if (entity is not null)
                _dbContext.Entry(entity).State = EntityState.Detached;

            return entity;
        }

        public async Task CreateAsync(T entity)
        {
            _entities.Add(entity);

            await SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _entities.Update(entity);

            await SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _entities.Remove(entity);

            await SaveChangesAsync();
        }

        public async Task DeleteManyAsync(List<T> entities)
        {
            _entities.RemoveRange(entities);

            await SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task BeginTransaction()
        {
            _transaction = await _dbContext.Database.BeginTransactionAsync();
        }

        public async Task CommitTransaction()
        {
            if (_transaction is not null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
            }
        }

        public async Task RollbackTransaction()
        {
            if (_transaction is not null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
            }
        }

    }
}