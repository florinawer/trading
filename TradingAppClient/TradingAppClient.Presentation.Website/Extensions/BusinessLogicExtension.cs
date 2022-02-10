using Microsoft.Extensions.DependencyInjection;
using TradingAppClient.Business.Logic.Interfaces;

namespace TradingAppClient.Presentation.Website.Extensions
{
    public static class BusinessLogicExtension
    {
        public static void ConfigureBusinessLogic(this IServiceCollection services)
        {
            //injección de dependencias 
            //añadir interface e implementación
            services.AddScoped<IStockBL, StockBL>();
            services.AddScoped<IPortfolioStockBL, PortfolioStockBL>();
        }
    }
}
