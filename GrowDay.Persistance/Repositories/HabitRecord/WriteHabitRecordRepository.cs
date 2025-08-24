using GrowDay.Application.Repositories;
using GrowDay.Domain.Entities.Concretes;
using GrowDay.Persistance.DbContexts;
using GrowDay.Persistance.Repositories.Common;

namespace GrowDay.Persistance.Repositories
{
    public class WriteHabitRecordRepository : WriteGenericRepository<HabitRecord>, IWriteHabitRecordRepository
    {
        public WriteHabitRecordRepository(GrowDayDbContext context) : base(context)
        {
        }
    }
}
