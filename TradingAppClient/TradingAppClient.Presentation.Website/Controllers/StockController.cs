using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TradingAppClient.Business.Logic.Interfaces;
using TradingAppClient.Common.Dto.Pagination;
using TradingAppClient.Presentacion.Website.Pagination;
using TradingAppClient.Presentation.Website.Models;

namespace TradingAppClient.Presentation.Website.Controllers
{
    public class StockController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IStockBL _stockBl;

        public StockController(IStockBL stockBl, IMapper mapper)
        {
            //mapper para convertir el dto a stockviewModel
            _mapper = mapper;
            //stock business logic para acceder a la base de datos 
            //y capturar los datos
            _stockBl = stockBl;
        }

        //en la pagina de index se llama a esta acción
        [HttpGet]
        public async Task<IActionResult> Index([Bind("PageNumber", "PageSize")] PaginationFilterViewModel paginatorFilter)
        {
            var result = await _stockBl.GetAll(_mapper.Map<PaginationFilterDto>(paginatorFilter));

            return View(_mapper.Map<PagedResponseViewModel<IEnumerable<StockViewModel>>>(result));
        }
    }
}
