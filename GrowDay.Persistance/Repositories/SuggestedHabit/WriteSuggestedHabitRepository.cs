using GrowDay.Application.Repositories;
using GrowDay.Domain.Entities.Concretes;
using GrowDay.Persistance.DbContexts;
using GrowDay.Persistance.Repositories.Common;

namespace GrowDay.Persistance.Repositories
{
    public class WriteSuggestedHabitRepository : WriteGenericRepository<SuggestedHabit>, IWriteSuggestedHabitRepository
    {
        public WriteSuggestedHabitRepository(GrowDayDbContext context) : base(context)
        {
        }
    }
}
