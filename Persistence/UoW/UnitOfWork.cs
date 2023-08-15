using Infrastructure.Contracts.Repositories;
using Infrastructure.Contracts.UoW;
using Infrastructure.Data;
using Infrastructure.Repositories;

namespace Persistence.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;

        public UnitOfWork(DataContext context)
        {
            _context = context;
            Categories = new CategoriesRepository(_context);
        }

        #region Repositories

        public ICategoriesRepository Categories { get; private set; }

        #endregion

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
