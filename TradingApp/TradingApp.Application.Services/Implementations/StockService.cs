using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TradingApp.Application.Services.Interfaces;
using TradingApp.Domain.Entities;
using TradingApp.Domain.Entities.Pagination;
using TradingApp.Infrastructure.Data.Model;
using TradingApp.Infrastructure.Repository;
using TradingApp.Infrastructure.Repository.Interfaces;

namespace TradingApp.Application.Services.Implementations
{
    public class StockService : IStockService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStockRepository _stockRepository;
        private readonly IMapper _mapper;

        public StockService(IUnitOfWork unitOfWork, IStockRepository stockRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _stockRepository = stockRepository;
            _mapper = mapper;
        }

        public async Task<StockEntity> Get(int id)
        {
            return _mapper.Map<StockEntity>(await _stockRepository.GetByIdAsync(id));
        }

        public async Task<IEnumerable<StockEntity>> GetAll()
        {
            return _mapper.Map<IEnumerable<StockEntity>>(await _stockRepository.GetAllAsync());
        }

        public async Task<PagedResponseEntity<IEnumerable<StockEntity>>> GetPaged(PaginationFilterEntity paginationFilter)
        {
            var totalResults = await _stockRepository.GetCountAsync();
            double totalPages = (double)totalResults / paginationFilter.PageSize;
            var data = _mapper.Map<IEnumerable<StockEntity>>(_stockRepository
                .GetPaginatedSync(paginationFilter.PageNumber, paginationFilter.PageSize));

            return new()
            {
                Data = data,
                TotalPages = (int)Math.Ceiling(totalPages),
                TotalRecords = totalResults,
                PageNumber = paginationFilter.PageNumber,
                PageSize = paginationFilter.PageSize
            };
        }

        public async Task<StockEntity> Post(StockEntity stockEntity)
        {
            var response = await _stockRepository.InsertAsync(_mapper.Map<Stock>(stockEntity));
            _unitOfWork.Commit();
            return _mapper.Map<StockEntity>(response);
        }

        public async Task<StockEntity> Put(int id, StockEntity stockEntity)
        {
            var stock = _mapper.Map<Stock>(stockEntity);
            stock.StockId = id;
            var response = await _stockRepository.UpdateAsync(stock);
            _unitOfWork.Commit();
            return _mapper.Map<StockEntity>(response);
        }
        public Task<bool> Delete(int id)
        {
            var response = _stockRepository.RemoveByIdAsync(id);
            _unitOfWork.Commit();
            return response;
        }
    }
}
