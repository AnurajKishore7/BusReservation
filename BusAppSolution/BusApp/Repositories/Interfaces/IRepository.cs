namespace BusApp.Repositories.Interfaces
{
    public interface IRepository<T, K> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByKeyAsync(K key);
        Task AddAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(K key);
    }
}
