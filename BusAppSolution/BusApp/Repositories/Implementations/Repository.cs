using BusApp.Repositories.Interfaces;
using BusReservationApp.Context;
using Microsoft.EntityFrameworkCore;

namespace BusApp.Repositories.Implementations
{
    public class Repository<T, K> : IRepository<T, K> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                return await _dbSet.ToListAsync();
            }
            catch (Exception)
            {
                throw new Exception($"An error occurred while fetching all records of type {typeof(T).Name}.");
            }
        }

        public async Task<T?> GetByKeyAsync(K key)
        {
            try
            {
                return await _dbSet.FindAsync(key);
            }
            catch (Exception)
            {
                throw new Exception($"An error occurred while fetching the entity of type {typeof(T).Name} with key {key}.");
            }
        }

        public async Task AddAsync(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception($"An error occurred while adding a new entity of type {typeof(T).Name}.");
            }
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            try
            {
                _dbSet.Update(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                throw new Exception($"An error occurred while updating the entity of type {typeof(T).Name}.");
            }
        }

        public async Task<bool> DeleteAsync(K key)
        {
            try
            {
                var entity = await GetByKeyAsync(key);
                if (entity == null)
                    throw new Exception($"Entity of type {typeof(T).Name} with key {key} not found.");

                _dbSet.Remove(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                throw new Exception($"An error occurred while deleting the entity of type {typeof(T).Name} with key {key}.");
            }
        }
    }
}
