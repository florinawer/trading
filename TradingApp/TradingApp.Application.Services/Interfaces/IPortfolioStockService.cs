using System.Collections.Generic;
using System.Threading.Tasks;
using TradingApp.Domain.Entities;
using TradingApp.Domain.Entities.Pagination;

namespace TradingApp.Application.Services.Interfaces
{
    public interface IPortfolioStockService
    {
        public Task<IEnumerable<PortfolioStockEntity>> GetAll();
        public Task<PortfolioStockEntity> Get(int id);
        public Task<PortfolioStockEntity> Post(PortfolioStockEntity stockEntity);
        public Task<PortfolioStockEntity> Put(int id, PortfolioStockEntity stockEntity);
        public Task<bool> Delete(int id);
        public Task<PagedResponseEntity<IEnumerable<PortfolioStockEntity>>> GetPaged(PaginationFilterEntity paginationFilter);
    }
}
