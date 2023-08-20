using Infrastructure.Contracts.Repositories;
using Infrastructure.Data;
using Infrastructure.Entities;
using Infrastructure.Repositories.Base;

namespace Infrastructure.Repositories
{
    public class RolesRepository : RepositoryBase<Role>, IRolesRepository
    {
        public RolesRepository(DataContext context) : base(context)
        {
        }
    }
}
