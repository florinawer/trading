using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using TradingAppClient.Common.Dto;
using TradingAppClient.Common.Dto.Pagination;
using TradingAppClient.Presentacion.Website.Pagination;
using TradingAppClient.Presentation.Website.Models;

namespace TradingAppClient.Presentation.Website.Extensions
{
    public static class AutoMapperExtension
    {
        //recibe los services por parametro y se configura
        public static void ConfigureAutoMapper(this IServiceCollection services)
        {
            IMapper mapper = GetMapperConfiguration().CreateMapper();
             
            //creacion de instancia única del mapper
            services.AddSingleton(mapper);
        }
        private static MapperConfiguration GetMapperConfiguration()
        {
            return new(mapperConfiguration =>
            {
                //mapeado entre objetos dto y entities
                mapperConfiguration
                    .CreateMap<StockDTO, StockViewModel>()
                    .ReverseMap();
                mapperConfiguration
                    .CreateMap<PortfolioStockDTO, PortfolioStockViewModel>()
                    .ForMember(dest => dest.Stock, opt =>
                    {
                        opt.MapFrom(src => src.StockDTO);
                    })
                    .ReverseMap();

                mapperConfiguration
                    .CreateMap(typeof(PagedResponseDto<>), typeof(PagedResponseViewModel<>))
                    .ReverseMap();

                mapperConfiguration
                    .CreateMap<PaginationFilterDto, PaginationFilterViewModel>()
                    .ReverseMap();
            });
        }
    }
}
