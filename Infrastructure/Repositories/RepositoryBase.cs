using Infrastructure.Contracts.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
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

        public virtual async Task<ResultQuery<TEntity>> Query(Expression<Func<TEntity, bool>>? predicate = null, int page = 0, int pageSize = 0)
        {
            var query = _dbSet.AsQueryable().AsNoTracking();

            if (predicate != null)
                query = query.Where(predicate);

            int total = 0;

            if (pageSize > 0)
            {
                if (pageSize > 100)
                    pageSize = 100;

                total = query.Count();

                query = query.Skip(pageSize * page).Take(pageSize);
            }

            var results = await query.ToListAsync();

            return new ResultQuery<TEntity> { Results = results, TotalRecords = total };
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
