using GrowDay.Application.Repositories;
using GrowDay.Domain.Entities.Concretes;
using GrowDay.Persistance.DbContexts;
using GrowDay.Persistance.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace GrowDay.Persistance.Repositories
{
    public class ReadSuggestedHabitRepository : ReadGenericRepository<SuggestedHabit>, IReadSuggestedHabitRepository
    {
        public ReadSuggestedHabitRepository(GrowDayDbContext context) : base(context)
        {
        }

        public async Task<ICollection<SuggestedHabit>> GetSuggestedHabitsAsync(int pageIndex = 0, int pageSize = 10)
        {
            return await _table
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<ICollection<SuggestedHabit>> GetSuggestedUserHabitsAsync(int pageIndex = 0, int pageSize = 10)
        {
            return await _table
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
