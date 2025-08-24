using GrowDay.Application.Repositories;
using GrowDay.Domain.Entities.Concretes;
using GrowDay.Persistance.DbContexts;
using GrowDay.Persistance.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace GrowDay.Persistance.Repositories
{
    public class ReadHabitRecordRepository : ReadGenericRepository<HabitRecord>, IReadHabitRecordRepository
    {
        public ReadHabitRecordRepository(GrowDayDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<HabitRecord>> GetAllByHabitIdAsync(string habitId)
        {
            return await _table
                .Where(hr => hr.HabitId == habitId)
                .ToListAsync();
        }

        public async Task<IEnumerable<HabitRecord>> GetAllByUserAndDateRangeAsync(string userId, DateTime startDate, DateTime endDate)
        {
            return await _table
                .Include(hr => hr.Habit)
                    .ThenInclude(h => h.UserHabits)
                .Where(hr => hr.Habit.UserHabits != null &&
                             hr.Habit.UserHabits.Any(uh => uh.UserId == userId && !uh.IsDeleted) &&
                             hr.Date >= startDate && hr.Date <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<HabitRecord>> GetAllByUserAsync(string userId)
        {
            return await _table
                .Include(hr => hr.Habit)
                    .ThenInclude(h => h.UserHabits)
                .Where(hr => hr.Habit.UserHabits != null &&
                             hr.Habit.UserHabits.Any(uh => uh.UserId == userId && !uh.IsDeleted))
                .ToListAsync();


        }
    }
}
