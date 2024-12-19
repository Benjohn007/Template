using BillCollector.Core.Entities;
using BillCollector.Infrastructure.Repository;

namespace BillCollector.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork
    {
        GenericRepository<User> User { get; }


        Task<int> CommitAsync();
        Task RollBackAsync();
    }
}
