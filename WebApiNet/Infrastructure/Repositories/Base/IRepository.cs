using WebApiNet.Presentation.Paged;

namespace WebApiNet.Infrastructure.Repositories.Base
{
    public interface IRepository<T, Tkey> where T : class
    {
        Task<PagedResult<T>> GetAllAsync(int pageNumber, int pageSize);
        Task<T?> GetByIdAsync(Tkey id);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(Tkey id,T entity);
        Task<bool> DeleteAsync(Tkey id);
    }
}
