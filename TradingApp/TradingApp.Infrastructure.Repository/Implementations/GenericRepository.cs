using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradingApp.Infrastructure.Repository;
using TradingApp.Infrastructure.Repository.Interfaces;

namespace CapgeminiDDD.Infrastructure.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly IUnitOfWork _unitOfWork;

        public GenericRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _unitOfWork.Context.Set<T>().ToListAsync();
        }
        public virtual IEnumerable<T> GetPaginatedSync(int pageNumber, int pageSize)
        {
            return _unitOfWork.Context.Set<T>()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _unitOfWork.Context.Set<T>().FindAsync(id);
        }

        public virtual async Task<T> InsertAsync(T entity)
        {
            await _unitOfWork.Context.AddAsync(entity);
            return entity;
        }

        public virtual async Task<bool> RemoveByIdAsync(int id)
        {
            var dbEntity = await _unitOfWork.Context.Set<T>().FindAsync(id);
            if (dbEntity != null)
            {
                _unitOfWork.Context.Set<T>().Remove(dbEntity);
                return true;
            }
            return false;
        }

        public virtual Task<T> UpdateAsync(T identity)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<int> GetCountAsync()
        {
            return _unitOfWork.Context.Set<T>().CountAsync();
        }

    }
}
