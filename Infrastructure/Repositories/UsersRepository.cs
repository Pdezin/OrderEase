using Infrastructure.Contracts.Repositories;
using Infrastructure.Data;
using Infrastructure.Entities;
using Infrastructure.Repositories.Base;

namespace Infrastructure.Repositories
{
    public class UsersRepository : RepositoryBase<User>, IUsersRepository
    {
        public UsersRepository(DataContext context) : base(context)
        {
        }
    }
}
