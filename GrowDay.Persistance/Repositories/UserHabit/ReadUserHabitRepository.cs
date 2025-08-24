using GrowDay.Application.Repositories;
using GrowDay.Domain.Entities.Concretes;
using GrowDay.Persistance.DbContexts;
using GrowDay.Persistance.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace GrowDay.Persistance.Repositories
{
    public class ReadUserHabitRepository:ReadGenericRepository<UserHabit>, IReadUserHabitRepository
    {
        public ReadUserHabitRepository(GrowDayDbContext context) : base(context)
        {
        }

        public async Task<UserHabit?> GetByUserAndHabitAsync(string userId, string idOrTitleOrHabitId)
        {
            return await _table
                .Include(uh => uh.Habit)
                .FirstOrDefaultAsync(uh => uh.UserId == userId &&
                    (uh.Id == idOrTitleOrHabitId || uh.HabitId == idOrTitleOrHabitId || uh.Title == idOrTitleOrHabitId));
        }



        public async Task<IEnumerable<UserHabit>> GetByUserIdAsync(string userId)
        {
            return await _table
                .Include(uh => uh.Habit)
                .Where(uh => uh.UserId == userId && !uh.IsDeleted)
                .ToListAsync();
        }
    }
}
