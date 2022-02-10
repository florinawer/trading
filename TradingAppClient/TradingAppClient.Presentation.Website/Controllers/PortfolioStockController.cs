using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TradingAppClient.Business.Logic.Interfaces;
using TradingAppClient.Common.Dto;
using TradingAppClient.Presentation.Website.Models;

namespace TradingAppClient.Presentation.Website.Controllers
{
    public class PortfolioStockController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IPortfolioStockBL _portfolioStockBl;

        public PortfolioStockController(IPortfolioStockBL portfolioStockBl, IMapper mapper)
        {
            _mapper = mapper;
            _portfolioStockBl = portfolioStockBl;
        }
        public async Task<ActionResult> Index()
        {
            var result = await _portfolioStockBl.GetAllAsync();
            return View(_mapper.Map<IEnumerable<PortfolioStockViewModel>>(result));
        }

        //traslada a la vista/controlador actual la información 
        //del stock con su id y nombre 
        public ActionResult Create(string name, string ticket)
        {
            PortfolioStockViewModel portfolioStock = new();
            portfolioStock.Stock = new(){Name = name, Ticket = ticket};
            return View(portfolioStock);
        }

        //llamada a la AddAsync mapeando el objeto de portfoliosStockModelView
        //regireccionado a la vista de view
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(string name, string ticket, [Bind("Amount,BuyPrice,BuyDate")] PortfolioStockViewModel portfolioStock)
        {
            try
            {
                portfolioStock.Stock = new() { Name = name, Ticket = ticket};
                await _portfolioStockBl.AddAsync(_mapper.Map<PortfolioStockDTO>(portfolioStock));
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
