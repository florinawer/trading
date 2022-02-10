using NPOI.SS.Formula.Functions;
using System.Collections.Generic;
using System.Threading.Tasks;
using TradingApp.Domain.Entities;
using TradingApp.Domain.Entities.Pagination;

namespace TradingApp.Application.Services.Interfaces
{
    public interface IStockService
    {
        public Task<IEnumerable<StockEntity>> GetAll();
        public Task<StockEntity> Get(int id);
        public Task<StockEntity> Post(StockEntity stockEntity);
        public Task<StockEntity> Put(int id, StockEntity stockEntity);
        public Task<bool> Delete(int id);
        public Task<PagedResponseEntity<IEnumerable<StockEntity>>> GetPaged(PaginationFilterEntity paginationFilter);
    }
}
