using GrowDay.Application.Repositories;
using GrowDay.Domain.Entities.Concretes;
using GrowDay.Persistance.DbContexts;
using GrowDay.Persistance.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace GrowDay.Persistance.Repositories
{
    public class ReadHabitRepository : ReadGenericRepository<Habit>, IReadHabitRepository
    {
        public ReadHabitRepository(GrowDayDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Habit>> GetAllHabitsListAsync(int pageIndex = 0, int pageSize = 10)
        {
            return await _table
                .Where(h => !h.IsGlobal)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Habit?> GetByTitleAndUserAsync(string title, string userId)
        {
            return await _table
                .FirstOrDefaultAsync(h => h.Title == title && h.CreatedByUserId == userId && !h.IsGlobal);
        }
        public async Task<Habit?> GetByTitleAsync(string title)
        {
            return await _table
                .FirstOrDefaultAsync(h => h.Title == title && !h.IsGlobal);
        }
    }
}
