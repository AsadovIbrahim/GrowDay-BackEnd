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

        public async Task<IEnumerable<TaskEntity>> GetAllTasksAsync(int pageIndex = 0, int pageSize = 10)
        {
            return await _table
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<ICollection<TaskEntity>> GetByHabitIdAsync(string habitId)
        {
            return await _table.Where(t => t.HabitId == habitId).ToListAsync();
        }
    }
}
