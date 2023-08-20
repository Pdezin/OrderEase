using Infrastructure.Contracts.Repositories;
using Infrastructure.Data;
using Infrastructure.Entities;
using Infrastructure.Repositories.Base;

namespace Infrastructure.Repositories
{
    public class PriceListsRepository : RepositoryBase<PriceList>, IPriceListsRepository
    {
        public PriceListsRepository(DataContext context) : base(context)
        {
        }
    }
}
