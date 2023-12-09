using Microsoft.EntityFrameworkCore;
using Physiosoft.Data;

namespace Physiosoft.Repisotories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        public readonly PhysiosoftDbContext _context;
        private readonly DbSet<T> _table;

        public BaseRepository(PhysiosoftDbContext context)
        {
            _context = context;
            _table = _context.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            var entities = await _table.ToListAsync();
            return entities;
        }

        public virtual async Task<T?> GetAsync(int id)
        {
            var entity = await _table.FindAsync(id);
            return entity;
        }

        public virtual async Task AddAsync(T entity)
        {
            await _table.AddAsync(entity);
        }

        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _table.AddRangeAsync(entities);
        }

        public virtual void UpdateAysnc(T entity)
        {
            // _table.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            T? existingEntity = await _table.FindAsync(id);

            if (existingEntity != null) return false;

            _table.Remove(existingEntity);
            return true;
        }

        public virtual async Task<int> GetCountAsync()
        {
            var count = await _table.CountAsync();
            return count;
        }
    }
}
