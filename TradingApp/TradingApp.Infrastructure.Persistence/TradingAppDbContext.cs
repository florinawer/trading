using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TradingApp.Infrastructure.Data.Model;

namespace TradingApp.Infrastructure.Persistence
{
    public class TradingAppDbContext : DbContext
    {
        public DbSet<Stock> Stocks { get; set; }

        public DbSet<PortfolioStock> PortfolioStocks { get; set; }

        public TradingAppDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseMySQL(config.GetConnectionString("TradingAppDataContextString"));
            }
        }
    }
}
