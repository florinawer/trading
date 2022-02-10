using AutoMapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TradingAppClient.Common.Dto;
using TradingAppClient.Common.Dto.Pagination;

namespace TradingAppClient.Business.Logic.Interfaces
{
    public class StockBL : IStockBL
    {
        //creacion instancia de la HttpClient
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        //la configuración llega por parametro así tener acceso al fichero
        public StockBL(HttpClient httpClient, IConfiguration config)
        {
            _configuration = config;
            //setea la base address a la dirección guardada en appsettings.json 
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(config.GetSection("TradingApiURL").Value);
        }      

        public async Task<PagedResponseDto<IEnumerable<StockDTO>>> GetAll(PaginationFilterDto paginationFilter) 
        {
            var httpResponse = await _httpClient.GetFromJsonAsync<PagedResponseDto<IEnumerable<StockDTO>>>($"/api/Stock/Paged?pageNumber={paginationFilter.PageNumber}&pageSize={paginationFilter.PageSize}");
            
            return httpResponse;
        }
    }
}
