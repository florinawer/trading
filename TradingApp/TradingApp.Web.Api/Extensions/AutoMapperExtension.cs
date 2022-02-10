using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using TradingApp.Domain.Entities;
using TradingApp.Domain.Entities.Pagination;
using TradingApp.Infrastructure.Data.Model;
using TradingApp.Web.Dto;
using TradingApp.Web.Dto.Pagination;

namespace TradingApp.Web.Api.Extensions
{
    public static class AutoMapperExtension
    {
        public static void ConfigureAutoMapper(this IServiceCollection services)
        {
            IMapper mapper = GetMapperConfiguration().CreateMapper();
            services.AddSingleton(mapper);
        }

        private static MapperConfiguration GetMapperConfiguration()
        {
            return new(mapperConfiguration =>
            {
                mapperConfiguration
                    .CreateMap<StockDTO, StockEntity>()
                    .ReverseMap();
                mapperConfiguration
                    .CreateMap<StockEntity, Stock>()
                    .ReverseMap();

                mapperConfiguration
                    .CreateMap<PortfolioStockDTO, PortfolioStockEntity>()
                    .ForMember(dest => dest.StockEntity, opt =>
                    {
                        opt.MapFrom(src => src.StockDTO);
                    })
                    .ReverseMap();
                mapperConfiguration
                    .CreateMap<PortfolioStockEntity, PortfolioStock>()
                    .ForMember(dest => dest.Stock, opt =>
                    {
                        opt.MapFrom(src => src.StockEntity);
                    })
                    .ReverseMap();
                //mapeado entre paginators
                //typeof porque es objeto generico y hay que hacer el mapeado a mano
                mapperConfiguration
                    .CreateMap(typeof(PagedResponseDTO<>), typeof(PagedResponseEntity<>))
                    .ReverseMap();
                mapperConfiguration
                    .CreateMap<PaginationFilterDTO, PaginationFilterEntity>()
                    .ReverseMap();
               
            });
        }
    }
}
