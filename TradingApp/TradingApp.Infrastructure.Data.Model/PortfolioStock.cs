using System;

namespace TradingApp.Infrastructure.Data.Model
{
    public class PortfolioStock
    {
        public int PortfolioStockId { get; set; }

        public int Amount { get; set; }

        public int BuyPrice { get; set; }

        public DateTime BuyDate { get; set; }

        public int StockId { get; set; }

        public Stock Stock { get; set; }
    }
}
