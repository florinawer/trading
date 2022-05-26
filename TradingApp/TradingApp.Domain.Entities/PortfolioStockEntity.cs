using System;

namespace TradingApp.Domain.Entities
{
    public class PortfolioStockEntity
    {
        public StockEntity StockEntity { get; set; }

        public int Amount { get; set; }

        public int BuyPrice { get; set; }

        public DateTime BuyDate { get; set; }
    }
}