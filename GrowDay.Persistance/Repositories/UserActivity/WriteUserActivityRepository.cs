using GrowDay.Application.Repositories;
using GrowDay.Domain.Entities.Concretes;
using GrowDay.Persistance.DbContexts;
using GrowDay.Persistance.Repositories.Common;

namespace GrowDay.Persistance.Repositories
{
    public class WriteUserActivityRepository : WriteGenericRepository<UserActivity>, IWriteUserActivityRepository
    {
        public WriteUserActivityRepository(GrowDayDbContext context) : base(context)
        {
        }
    }
}
