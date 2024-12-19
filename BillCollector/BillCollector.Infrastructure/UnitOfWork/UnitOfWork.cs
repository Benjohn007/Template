using BillCollector.Core.Entities;
using BillCollector.Infrastructure.DbContexts;
using BillCollector.Infrastructure.Repository;

namespace BillCollector.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BillCollectorDbContext _context;

        public UnitOfWork(BillCollectorDbContext context)
        {
            _context = context;
        }

        #region Declarations -- Expand to see more

        private GenericRepository<User> _userRepository;

        #endregion


        #region Implementation -- Expand to see more

        public GenericRepository<User> User => _userRepository ??= new GenericRepository<User>(_context);
        #endregion

        public async Task<int> CommitAsync()
        {
            try
            {
                return (await _context.SaveChangesAsync());
            }
            catch (Exception)
            {
                await RollBackAsync();
            }
            return 0;
        }

        public async Task RollBackAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }

    }
}
