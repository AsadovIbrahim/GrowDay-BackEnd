using GrowDay.Application.Repositories;
using GrowDay.Domain.Entities.Concretes;
using GrowDay.Persistance.DbContexts;
using GrowDay.Persistance.Repositories.Common;

namespace GrowDay.Persistance.Repositories
{
    public class WriteHabitRepository : WriteGenericRepository<Habit>, IWriteHabitRepository
    {
        public WriteHabitRepository(GrowDayDbContext context) : base(context)
        {
        }
    }
}
