using Infrastructure.Contracts.Repositories;
using Infrastructure.Data;
using Infrastructure.Entities;
using Infrastructure.Repositories.Base;

namespace Infrastructure.Repositories
{
    public class ProductsRepository : RepositoryBase<Product>, IProductsRepository
    {
        public ProductsRepository(DataContext context) : base(context)
        {
        }
    }
}
