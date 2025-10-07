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

        public async Task<IEnumerable<HabitRecord>> GetAllByHabitIdAsync(string userHabitId)
        {
            return await _table
                .Where(hr => hr.UserHabitId == userHabitId)
                .ToListAsync();
        }

        public async Task<IEnumerable<HabitRecord>> GetAllByUserAndDateRangeAsync(string userId, DateTime startDate, DateTime endDate)
        {
            return await _table
                .Include(hr => hr.UserHabit)
                    .ThenInclude(uh => uh.Habit)
                .Where(hr => hr.UserHabit != null &&
                             hr.UserHabit.UserId == userId &&
                             hr.Date >= startDate && hr.Date <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<HabitRecord>> GetAllByUserAsync(string userId)
        {
            return await _table
                .Include(hr => hr.UserHabit)
                    .ThenInclude(uh => uh.Habit)
                .Where(hr => hr.UserHabit != null &&
                             hr.UserHabit.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<HabitRecord>> GetAllUserHabitIdAsync(string userHabitId)
        {
            return await _table
                .Where(hr => hr.UserHabitId == userHabitId)
                .ToListAsync();
        }

        public async Task<HabitRecord?> GetByUserHabitIdAndDateAsync(string userHabitId, DateTime date)
        {
            return await _table
                .FirstOrDefaultAsync(hr => hr.UserHabitId == userHabitId && hr.Date.Date == date.Date);
        }
    }
}
