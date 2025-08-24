using Microsoft.EntityFrameworkCore;
using GrowDay.Persistance.DbContexts;
using GrowDay.Domain.Entities.Abstracts;
using GrowDay.Application.Repositories.Common;

namespace GrowDay.Persistance.Repositories.Common
{
    public class ReadGenericRepository<T> : GenericRepository<T>, IReadGenericRepository<T> where T : class, IBaseEntity, new()
    {
        public ReadGenericRepository(GrowDayDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<T>?> GetAllAsync()
        {
            return await _table
                .Where(x=>x.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<T?> GetByIdAsync(string id)
        {
            return await _table.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
