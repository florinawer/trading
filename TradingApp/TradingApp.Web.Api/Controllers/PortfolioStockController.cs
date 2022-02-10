using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using TradingApp.Application.Services.Interfaces;
using TradingApp.Domain.Entities;
using TradingApp.Domain.Entities.Pagination;
using TradingApp.Web.Dto;
using TradingApp.Web.Dto.Pagination;

namespace TradingApp.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfolioStockController : ControllerBase
    {
        private readonly IPortfolioStockService _portfolioStockService;
        private readonly IMapper _mapper;
        private readonly ILogger<PortfolioStockController> _logger;
        public PortfolioStockController(IPortfolioStockService portfolioStockService, IMapper mapper, ILogger<PortfolioStockController> logger)
        {
            _portfolioStockService = portfolioStockService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<PortfolioStockDTO>> Get()
        {
            return _mapper.Map<IEnumerable<PortfolioStockDTO>>(await _portfolioStockService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<PortfolioStockDTO> Get(int id)
        {
            return _mapper.Map<PortfolioStockDTO>(await _portfolioStockService.Get(id));
        }

        [HttpGet("Paged")]
        public async Task<PagedResponseDTO<IEnumerable<PortfolioStockDTO>>> GetPaged([FromQuery] PaginationFilterDTO paginationFilter)
        {
            return _mapper
                .Map<PagedResponseDTO<IEnumerable<PortfolioStockDTO>>>(
                    await _portfolioStockService.GetPaged(_mapper.Map<PaginationFilterEntity>(paginationFilter)));
        }
        [HttpPost]
        public async Task<PortfolioStockDTO> Post([FromBody] PortfolioStockDTO stock)
        {
            //guarda en un archivo de texto cada vez que pasa por allí
            _logger.LogInformation($"Post ha sido invocado con valores {stock.StockDTO.Name} a precio de  {stock.BuyPrice}");
            
            var response = await _portfolioStockService.Post(_mapper.Map<PortfolioStockEntity>(stock));
            return _mapper.Map<PortfolioStockDTO>(response);
        }

        [HttpPut("{id}")]
        public async Task<PortfolioStockDTO> Put(int id, [FromBody] PortfolioStockDTO stock)
        {
            var response = await _portfolioStockService.Put(id, _mapper.Map<PortfolioStockEntity>(stock));
            return _mapper.Map<PortfolioStockDTO>(response);
        }

        [HttpDelete("{id}")]
        public Task<bool> Delete(int id)
        {
            return _portfolioStockService.Delete(id);
        }
    }
}
