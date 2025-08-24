using GrowDay.Application.Repositories;
using GrowDay.Domain.Entities.Concretes;
using GrowDay.Persistance.DbContexts;
using GrowDay.Persistance.Repositories.Common;

namespace GrowDay.Persistance.Repositories
{
    public class WriteUserHabitRepository : WriteGenericRepository<UserHabit>, IWriteUserHabitRepository
    {
        public WriteUserHabitRepository(GrowDayDbContext context) : base(context)
        {
        }
    }
}
