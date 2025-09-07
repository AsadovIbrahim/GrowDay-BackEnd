using GrowDay.Application.Repositories;
using GrowDay.Domain.Entities.Concretes;
using GrowDay.Persistance.DbContexts;
using GrowDay.Persistance.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace GrowDay.Persistance.Repositories
{
    public class ReadTaskRepository : ReadGenericRepository<TaskEntity>, IReadTaskRepository
    {
        public ReadTaskRepository(GrowDayDbContext context) : base(context)
        {
        }

        public async Task<ICollection<TaskEntity>> GetByHabitIdAsync(string habitId)
        {
            return await _table.Where(t => t.HabitId == habitId).ToListAsync();
        }
    }
}
