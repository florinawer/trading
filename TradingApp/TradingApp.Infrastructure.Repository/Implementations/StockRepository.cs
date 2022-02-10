using CapgeminiDDD.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradingApp.Infrastructure.Data.Model;
using TradingApp.Infrastructure.Repository.Interfaces;

namespace TradingApp.Infrastructure.Repository.Implementations
{
    public class StockRepository : GenericRepository<Stock>, IStockRepository
    {
        public StockRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<Stock> GetByTicket(string ticket)
        {
            return await _unitOfWork.Context
                .Stocks
                .FirstOrDefaultAsync(_ => _.Ticket == ticket);
        }
        public override async Task<Stock> UpdateAsync(Stock stock)
        {
            var dbStock = await _unitOfWork.Context
                .Stocks
                .FirstOrDefaultAsync(_ => _.StockId == stock.StockId);

            if (dbStock != null)
            {
                dbStock.Ticket = stock.Ticket;
                dbStock.Name = stock.Name;
            }

            return dbStock;
        }

        public async Task BulkInsert(ICollection<Stock> stocks)
        {
            var query = "INSERT INTO Stocks(Name,Ticket) VALUES ";
            var initialLength = query.Length;

            foreach (Stock stock in stocks)
            {
                query += $"('{stock.Name}','{stock.Ticket}'),";
            }
            query = query.Remove(query.Length - 1);

            if (query.Length > initialLength)
                await _unitOfWork.Context.Database.ExecuteSqlRawAsync(query);
        }
        public async Task<IEnumerable<Stock>> GetPaginatedAsync(int pageNumber, int pageSize)
        {
            //método generico para utilizar en mas de una entidad
            //se sobreescribe el metodo virtual si fuera necesario
            List<Stock> stocks = new();
            var stock = await _unitOfWork.Context.Set<Stock>()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            stocks = stock;
            return stocks;
        }
    }
}
