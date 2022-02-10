using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TradingAppClient.Common.Dto;

namespace TradingAppClient.Business.Logic.Interfaces
{
    public class PortfolioStockBL : IPortfolioStockBL
    {
        private readonly HttpClient _httpClient;

        public PortfolioStockBL(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(config.GetSection("TradingApiURL").Value);
        }

        public async Task<PortfolioStockDTO> AddAsync(PortfolioStockDTO portfolioStock)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/PortfolioStock", portfolioStock);
            return await response.Content.ReadFromJsonAsync<PortfolioStockDTO>();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/PortfolioStock/{id}");
            return await response.Content.ReadFromJsonAsync<bool>();
        }

        public async Task<ICollection<PortfolioStockDTO>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync($"/api/PortfolioStock");
            return await response.Content.ReadFromJsonAsync<ICollection<PortfolioStockDTO>>();
        }
    }
}
