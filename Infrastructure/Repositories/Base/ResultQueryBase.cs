namespace Infrastructure.Repositories.Base
{
    public class QueryResult<TEntity>
    {
        public IEnumerable<TEntity> Results { get; set; } = Enumerable.Empty<TEntity>();
        public int TotalRecords { get; set; }
    }
}
