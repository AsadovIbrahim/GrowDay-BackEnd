using GrowDay.Application.Repositories;
using GrowDay.Domain.Entities.Concretes;
using GrowDay.Persistance.DbContexts;
using GrowDay.Persistance.Repositories.Common;

namespace GrowDay.Persistance.Repositories
{
    public class ReadAchievementRepository : ReadGenericRepository<Achievement>, IReadAchievementRepository
    {
        public ReadAchievementRepository(GrowDayDbContext context) : base(context)
        {
        }
    }
}
