namespace Infrastructure.Repositories
{
    public class ResultQuery<TEntity>
    {
        public IEnumerable<TEntity> Results { get; set; } = Enumerable.Empty<TEntity>();
        public int TotalRecords { get; set; }
    }
}
