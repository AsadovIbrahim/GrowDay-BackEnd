using GrowDay.Application.Repositories;
using GrowDay.Domain.Entities.Concretes;
using GrowDay.Persistance.DbContexts;
using GrowDay.Persistance.Repositories.Common;

namespace GrowDay.Persistance.Repositories
{
    public class WriteAchievementRepository : WriteGenericRepository<Achievement>, IWriteAchievementRepository
    {
        public WriteAchievementRepository(GrowDayDbContext context) : base(context)
        {
        }
    }
}
