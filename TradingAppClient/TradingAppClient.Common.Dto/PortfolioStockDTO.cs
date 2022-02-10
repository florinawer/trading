using System;

namespace TradingAppClient.Common.Dto
{
    public class PortfolioStockDTO
    {
        public StockDTO StockDTO { get; set; }

        public int Amount { get; set; }

        public int BuyPrice { get; set; }

        public DateTime BuyDate { get; set; }

    }
}
