using Infrastructure.Contracts.Repositories;
using Infrastructure.Contracts.UoW;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;

        public UnitOfWork(DataContext context)
        {
            _context = context;
            Categories = new CategoriesRepository(_context);
            Roles = new RolesRepository(_context);
            PriceLists = new PriceListsRepository(_context);
            Users = new UsersRepository(_context);
            Products = new ProductsRepository(_context);
        }

        #region Repositories

        public ICategoriesRepository Categories { get; private set; }
        public IRolesRepository Roles { get; private set; }
        public IPriceListsRepository PriceLists { get; private set; }
        public IUsersRepository Users { get; private set; }
        public IProductsRepository Products { get; set; }

        #endregion

        public async Task Commit()
        {
            var entries = _context.ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            //Set created and updated date on entities
            foreach (var entityEntry in entries)
            {
                if (entityEntry.Entity.GetType()?.GetProperty("UpdatedAt") != null)
                    entityEntry.Entity.GetType()?.GetProperty("UpdatedAt")?.SetValue(entityEntry.Entity, DateTime.UtcNow);

                if (entityEntry.State == EntityState.Added)
                    if (entityEntry.Entity.GetType()?.GetProperty("CreatedAt") != null)
                        entityEntry.Entity.GetType()?.GetProperty("CreatedAt")?.SetValue(entityEntry.Entity, DateTime.UtcNow);
            }

            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
