using Infrastructure.Contracts.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Contracts.UoW
{
    public interface IUnitOfWork : IDisposable
    {
        Task Commit();
        DataContext Context { get; }
        ICategoriesRepository Categories { get; }
        IRolesRepository Roles { get; }
        IPriceListsRepository PriceLists { get; }
        IUsersRepository Users { get; }
        IProductsRepository Products { get; }
    }
}
