using Infrastructure.Contracts.Repositories;
using Infrastructure.Data;
using Infrastructure.Entities;

namespace Infrastructure.Repositories
{
    public class CategoriesRepository : RepositoryBase<Category>, ICategoriesRepository
    {
        public CategoriesRepository(DataContext context) : base(context)
        {
        }
    }
}
