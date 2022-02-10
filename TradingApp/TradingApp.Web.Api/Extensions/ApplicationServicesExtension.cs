using Microsoft.Extensions.DependencyInjection;
using TradingApp.Application.Services.Implementations;
using TradingApp.Application.Services.Interfaces;

namespace TradingApp.Web.Api.Extensions
{
    public static class ApplicationServicesConfiguration
    {
        public static void ConfigureApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IStockService, StockService>();
            services.AddScoped<IPortfolioStockService, PortfolioStockService>();
        }
    }
}
