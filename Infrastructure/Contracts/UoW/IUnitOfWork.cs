using Infrastructure.Contracts.Repositories;

namespace Infrastructure.Contracts.UoW
{
    public interface IUnitOfWork : IDisposable
    {
        Task Commit();
        ICategoriesRepository Categories { get; }
    }
}
