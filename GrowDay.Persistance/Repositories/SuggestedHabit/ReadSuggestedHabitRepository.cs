using GrowDay.Application.Repositories;
using GrowDay.Domain.Entities.Concretes;
using GrowDay.Persistance.DbContexts;
using GrowDay.Persistance.Repositories.Common;

namespace GrowDay.Persistance.Repositories
{
    public class ReadSuggestedHabitRepository : ReadGenericRepository<SuggestedHabit>, IReadSuggestedHabitRepository
    {
        public ReadSuggestedHabitRepository(GrowDayDbContext context) : base(context)
        {
        }
    }
}
