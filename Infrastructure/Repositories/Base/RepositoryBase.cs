using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Infrastructure.Data;
using Infrastructure.Helpers;
using Infrastructure.Contracts.Repositories.Base;

namespace Infrastructure.Repositories.Base
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        private readonly DbSet<TEntity> _dbSet;

        public RepositoryBase(DataContext context)
        {
            _dbSet = context.Set<TEntity>();
        }

        public virtual async Task Add(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual async Task AddRange(IEnumerable<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public virtual async Task<TEntity?> Find(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>>? predicate = null)
        {
            var query = _dbSet.AsQueryable().AsNoTracking();

            if (predicate != null)
                query = query.Where(predicate);

            return await query.ToListAsync();
        }

        public virtual async Task<QueryResult<TEntity>> Query(Expression<Func<TEntity, bool>>? predicate = null, int page = 0, int pageSize = 0, string orderBy = "", bool orderDesc = false)
        {
            var query = _dbSet.AsQueryable().AsNoTracking();

            if (predicate != null)
                query = query.Where(predicate);

            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                if (orderDesc)
                    query = query.CustomOrderByDescending(orderBy);
                else
                    query = query.CustomOrderBy(orderBy);
            }

            int total = 0;

            if (pageSize > 0)
            {
                if (pageSize > 100)
                    pageSize = 100;

                total = query.Count();

                query = query.Skip(pageSize * page).Take(pageSize);
            }

            var results = await query.ToListAsync();

            return new QueryResult<TEntity> { Results = results, TotalRecords = total };
        }

        public virtual void Remove(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual void RemoveRange(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public virtual void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }
    }
}
