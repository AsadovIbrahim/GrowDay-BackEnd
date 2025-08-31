using GrowDay.Persistance.DbContexts;
using GrowDay.Domain.Entities.Abstracts;
using GrowDay.Application.Repositories.Common;

namespace GrowDay.Persistance.Repositories.Common
{
    public class WriteGenericRepository<T> : GenericRepository<T>, IWriteGenericRepository<T> where T : class, IBaseEntity, new()
    {
        public WriteGenericRepository(GrowDayDbContext context) : base(context)
        {
        }
            
        public async Task AddAsync(T entity)
        {
            await _table.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _table.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _table.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(string id)
        {
            var entity = _table.FirstOrDefault(x => x.Id == id);
            if (entity != null)
            {
                _table.Remove(entity);
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            _table.RemoveRange(entities);   
            await _context.SaveChangesAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<T> entities)
        {
            _table.RemoveRange(entities);
            await _context.SaveChangesAsync();
        }


        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _table.Update(entity);
            await _context.SaveChangesAsync();
        }

 
    }
}
