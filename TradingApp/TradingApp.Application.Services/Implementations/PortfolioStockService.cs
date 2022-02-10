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
    public class PortfolioStockService : IPortfolioStockService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPortfolioStockRepository _portfolioStockRepository;
        private readonly IMapper _mapper;

        public PortfolioStockService(IUnitOfWork unitOfWork, IPortfolioStockRepository portfolioStockRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _portfolioStockRepository = portfolioStockRepository;
        }

        public Task<bool> Delete(int id)
        {
            var response = _portfolioStockRepository.RemoveByIdAsync(id);
            _unitOfWork.Commit();
            return response;
        }

        public async Task<PortfolioStockEntity> Get(int id)
        {
            return _mapper.Map<PortfolioStockEntity>(await _portfolioStockRepository.GetByIdAsync(id));
        }

        public async Task<IEnumerable<PortfolioStockEntity>> GetAll()
        {
            return _mapper.Map<IEnumerable<PortfolioStockEntity>>(await _portfolioStockRepository.GetAllAsync());
        }

        public async Task<PortfolioStockEntity> Post(PortfolioStockEntity stockEntity)
        {
            var response = await _portfolioStockRepository.InsertAsync(_mapper.Map<PortfolioStock>(stockEntity));
            _unitOfWork.Commit();
            return _mapper.Map<PortfolioStockEntity>(response);
        }
        public async Task<PagedResponseEntity<IEnumerable<PortfolioStockEntity>>> GetPaged(PaginationFilterEntity paginationFilter)
        {
            var totalResults = await _portfolioStockRepository.GetCountAsync();
            double totalPages = (double)totalResults / paginationFilter.PageSize;
            var data = _mapper.Map<IEnumerable<PortfolioStockEntity>>(_portfolioStockRepository
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
        public async Task<PortfolioStockEntity> Put(int id, PortfolioStockEntity stockEntity)
        {
            var stock = _mapper.Map<PortfolioStock>(stockEntity);
            stock.StockId = id;
            var response = await _portfolioStockRepository.UpdateAsync(stock);
            _unitOfWork.Commit();
            return _mapper.Map<PortfolioStockEntity>(response);
        }
    }
}
