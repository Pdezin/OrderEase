using Infrastructure.Contracts.Repositories;

namespace Infrastructure.Contracts.UoW
{
    public interface IUnitOfWork : IDisposable
    {
        Task Commit();
        ICategoriesRepository Categories { get; }
        IRolesRepository Roles { get; }
        IPriceListsRepository PriceLists { get; }
        IUsersRepository Users { get; }
    }
}
