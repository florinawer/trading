using TradingApp.Infrastructure.Persistence;

namespace TradingApp.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public TradingAppDbContext Context { get; }

        public UnitOfWork(TradingAppDbContext context)
        {
            Context = context;
        }

        public void Commit()
        {
            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}
