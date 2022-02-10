using System.Collections.Generic;
using System.Threading.Tasks;

namespace TradingApp.Infrastructure.Repository.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        public Task<IEnumerable<T>> GetAllAsync();
        public IEnumerable<T> GetPaginatedSync(int pageNumber, int pageSize);
        public Task<T> GetByIdAsync(int id);
        public Task<T> InsertAsync(T entity);
        public Task<bool> RemoveByIdAsync(int id);
        public Task<T> UpdateAsync(T identity);
        public Task<int> GetCountAsync();

    }
}
