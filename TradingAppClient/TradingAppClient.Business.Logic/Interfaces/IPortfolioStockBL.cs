using System.Collections.Generic;
using System.Threading.Tasks;
using TradingAppClient.Common.Dto;

namespace TradingAppClient.Business.Logic.Interfaces
{
    public interface IPortfolioStockBL
    {
        Task<ICollection<PortfolioStockDTO>> GetAllAsync();
        Task<PortfolioStockDTO> AddAsync(PortfolioStockDTO portfolioStock);
        Task<bool> DeleteAsync(int id);
    }
}
