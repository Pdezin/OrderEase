using Infrastructure.Repositories.Base;
using System.Linq.Expressions;

namespace Infrastructure.Contracts.Repositories.Base
{
    public interface IRepositoryBase<TEntity>
    {
        Task<TEntity?> Find(int id);
        Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>>? predicate = null);
        Task<QueryResult<TEntity>> Query(Expression<Func<TEntity, bool>>? predicate = null, int page = 0, int pageSize = 0, string orderBy = "", bool orderDesc = false);
        Task Add(TEntity entity);
        Task AddRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}
