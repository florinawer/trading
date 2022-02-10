using System;

namespace TradingAppClient.Presentation.Website.Models
{
    public class PortfolioStockViewModel
    {
        public StockViewModel Stock { get; set; }

        public int Amount { get; set; }

        public int BuyPrice { get; set; }

        public DateTime BuyDate { get; set; }
    }
}
