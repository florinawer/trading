using NPOI.SS.Formula.Functions;
using System.Collections.Generic;
using System.Threading.Tasks;
using TradingApp.Infrastructure.Data.Model;

namespace TradingApp.Infrastructure.Repository.Interfaces
{
    public interface IStockRepository : IGenericRepository<Stock>
    {
        public Task<Stock> GetByTicket(string ticket);

        public Task BulkInsert(ICollection<Stock> stocks);

    }
}
