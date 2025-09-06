using GrowDay.Application.Repositories;
using GrowDay.Domain.Entities.Concretes;
using GrowDay.Persistance.DbContexts;
using GrowDay.Persistance.Repositories.Common;

namespace GrowDay.Persistance.Repositories
{
    public class WriteUserAchievementRepository : WriteGenericRepository<UserAchievement>, IWriteUserAchievementRepository
    {
        public WriteUserAchievementRepository(GrowDayDbContext context) : base(context)
        {
        }
    }
}
