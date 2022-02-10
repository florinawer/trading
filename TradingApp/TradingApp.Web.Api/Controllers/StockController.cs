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
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService;
        private readonly IMapper _mapper;
        private readonly ILogger<StockController> _logger;

        public StockController(IStockService stockService, IMapper mapper, ILogger<StockController> logger)
        {
            _stockService = stockService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<StockDTO>> Get()
        {
            return _mapper.Map<IEnumerable<StockDTO>>(await _stockService.GetAll());
        }

        [HttpGet("Paged")]
        public async Task<PagedResponseDTO<IEnumerable<StockDTO>>> GetPaged([FromQuery] PaginationFilterDTO paginationFilter)
        {
            //guarda en un archivo de texto cada vez que pasa por allí
            _logger.LogInformation($"GetPaginated ha sido invocado con valores {paginationFilter.PageNumber} and {paginationFilter.PageSize}");

            return _mapper
                .Map<PagedResponseDTO<IEnumerable<StockDTO>>>(
                    await _stockService.GetPaged(_mapper.Map<PaginationFilterEntity>(paginationFilter)));
        }

        [HttpGet("{id}")]
        public async Task<StockDTO> Get(int id)
        {
            return _mapper.Map<StockDTO>(await _stockService.Get(id));
        }

        [HttpPost]
        public async Task<StockDTO> Post([FromBody] StockDTO stock)
        {
            var response = await _stockService.Post(_mapper.Map<StockEntity>(stock));
            return _mapper.Map<StockDTO>(response);
        }

        [HttpPut("{id}")]
        public async Task<StockDTO> Put(int id, [FromBody] StockDTO stock)
        {
            var response = await _stockService.Put(id, _mapper.Map<StockEntity>(stock));
            return _mapper.Map<StockDTO>(response);
        }

        [HttpDelete("{id}")]
        public Task<bool> Delete(int id)
        {
            return _stockService.Delete(id);
        }
    }
}
