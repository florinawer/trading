using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using TradingApp.Domain.Entities;
using TradingApp.Infrastructure.Data.Model;
using TradingApp.Infrastructure.Repository;
using TradingApp.Infrastructure.Repository.Interfaces;

namespace TradingApp.Application.Services.Helpers
{
    public class StockDbSeeder
    {
        public static async Task FetchStocks(IServiceProvider serviceProvider, int insertSize = -1)
        {

            IStockRepository _stockRepository = serviceProvider.GetService<IStockRepository>();
            IMapper _mapper = serviceProvider.GetService<IMapper>();
            IUnitOfWork _unitOfWork = serviceProvider.GetService<IUnitOfWork>();

            if (await _stockRepository.GetCountAsync() > 0) return;

            var data = await GetStockDataFromAPI(serviceProvider);
            var stocks = (List<StockEntity>)await GetStockEntitiesFromData(data);

            if (insertSize <= 0)
                insertSize = stocks.Count;

            for (int i = 0; i < stocks.Count; i += insertSize)
            {
                var stocksToInsert = stocks.GetRange(i, Math.Min(insertSize, stocks.Count - i));
                await _stockRepository.BulkInsert(_mapper.Map<ICollection<Stock>>(stocksToInsert));
            }
            _unitOfWork.Commit();
        }

        private static async Task<MemoryStream> GetStockDataFromAPI(IServiceProvider serviceProvider)
        {
            using (var client = serviceProvider.GetService<HttpClient>())
            {
                string apiKey = serviceProvider
                    .GetService<IConfiguration>()
                    .GetSection("StockApiKey")
                    .Value;

                client.BaseAddress = new Uri("https://www.alphavantage.co/");
                var response = await client.GetAsync($"query?function=LISTING_STATUS&apikey={apiKey}");
                return (MemoryStream) await response.Content.ReadAsStreamAsync();
            }
        }

        private static async Task<ICollection<StockEntity>> GetStockEntitiesFromData(MemoryStream data)
        {
            var stocks = new List<StockEntity>();

            using (var reader = new StreamReader(data))
            {
                reader.ReadLineAsync();
                while (!reader.EndOfStream)
                {
                    var line = (await reader.ReadLineAsync()).Split(',');

                    stocks.Add(new StockEntity
                    {
                        Ticket = line[0],
                        //replace caracter raro si no te pasas 3 dias compilando
                        Name = line[1].Replace("'", string.Empty)
                    });
                }
            }
            return stocks;
        }
    }
}
