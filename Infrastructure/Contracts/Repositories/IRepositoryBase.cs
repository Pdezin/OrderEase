using Infrastructure.Repositories;
using System.Linq.Expressions;

namespace Infrastructure.Contracts.Repositories
{
    public interface IRepositoryBase<TEntity>
    {
        Task<TEntity?> Find(int id);
        Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>>? predicate = null);
        Task<ResultQuery<TEntity>> Query(Expression<Func<TEntity, bool>>? predicate = null, int page = 0, int pageSize = 0);
        Task Add(TEntity entity);
        Task AddRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}
