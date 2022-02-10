using System.Collections.Generic;
using System.Threading.Tasks;
using TradingAppClient.Common.Dto;
using TradingAppClient.Common.Dto.Pagination;

namespace TradingAppClient.Business.Logic.Interfaces
{
    public interface IStockBL
    {
        Task<PagedResponseDto<IEnumerable<StockDTO>>> GetAll(PaginationFilterDto paginationFilter);
    }
}
