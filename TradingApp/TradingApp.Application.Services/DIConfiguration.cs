using Microsoft.Extensions.DependencyInjection;
using TradingApp.Infrastructure.Persistence;
using TradingApp.Infrastructure.Repository;
using TradingApp.Infrastructure.Repository.Implementations;
using TradingApp.Infrastructure.Repository.Interfaces;

namespace Indra.Application.Services
{
    public static class DIConfiguration
    {
        public static void ConfigureRepositories(this IServiceCollection services)
        {
            services.AddDbContext<TradingAppDbContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IStockRepository, StockRepository>();
            services.AddScoped<IPortfolioStockRepository, PortfolioStockRepository>();
        }
    }
}
