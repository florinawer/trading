using System;
using TradingApp.Infrastructure.Persistence;

namespace TradingApp.Infrastructure.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        public TradingAppDbContext Context { get; }

        public void Commit();
    }
}
