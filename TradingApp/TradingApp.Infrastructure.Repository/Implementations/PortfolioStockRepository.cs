using CapgeminiDDD.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradingApp.Infrastructure.Data.Model;
using TradingApp.Infrastructure.Repository.Interfaces;

namespace TradingApp.Infrastructure.Repository.Implementations
{
    public class PortfolioStockRepository : GenericRepository<PortfolioStock>, IPortfolioStockRepository
    {
        private readonly IStockRepository _stockRepository;

        public PortfolioStockRepository(IUnitOfWork unitOfWork, IStockRepository stockRepository) : base(unitOfWork)
        {
            _stockRepository = stockRepository;
        }

        public override async Task<PortfolioStock> InsertAsync(PortfolioStock entity)
        {
            var dbStock = await _stockRepository.GetByTicket(entity.Stock.Ticket);

            if (dbStock == null)
                dbStock = await _stockRepository.InsertAsync(entity.Stock);

            entity.Stock = dbStock;

            await _unitOfWork.Context.AddAsync(entity);

            return entity;
        }

        public override IEnumerable<PortfolioStock> GetPaginatedSync(int pageNumber, int pageSize)
        {
            return _unitOfWork.Context.PortfolioStocks
                .Skip((pageNumber - 1) * pageSize)
                .Include(_ => _.Stock)
                .Take(pageSize);
        }

        public override async Task<IEnumerable<PortfolioStock>> GetAllAsync()
        {
            return await _unitOfWork.Context
                .PortfolioStocks
                .Include(_ => _.Stock)
                .ToListAsync();
        }

        public override async Task<PortfolioStock> GetByIdAsync(int id)
        {
            return await _unitOfWork.Context
                .PortfolioStocks
                .Include(_ => _.Stock)
                .FirstOrDefaultAsync(_ => _.PortfolioStockId == id);
        }

        public override async Task<PortfolioStock> UpdateAsync(PortfolioStock portfolioStock)
        {
            var dbPortfolioStock = await _unitOfWork.Context
                .PortfolioStocks
                .Include(_ => _.Stock)
                .FirstOrDefaultAsync(_ => _.PortfolioStockId == portfolioStock.PortfolioStockId);

            if (dbPortfolioStock != null)
            {
                dbPortfolioStock.Amount = portfolioStock.Amount;
                dbPortfolioStock.BuyPrice = portfolioStock.BuyPrice;
                dbPortfolioStock.BuyDate = portfolioStock.BuyDate;
            }

            return dbPortfolioStock;
        }
    }
}
