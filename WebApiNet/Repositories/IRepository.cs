namespace WebApiNet.Repositories
{
    public interface IRepository<T, Tkey> where T : class
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T?> GetByIdAsync(Tkey id);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(Tkey id);
    }
}
