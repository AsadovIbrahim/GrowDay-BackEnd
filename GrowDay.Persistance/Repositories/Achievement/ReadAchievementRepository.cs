using GrowDay.Application.Repositories;
using GrowDay.Domain.Entities.Concretes;
using GrowDay.Persistance.DbContexts;
using GrowDay.Persistance.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace GrowDay.Persistance.Repositories
{
    public class ReadAchievementRepository : ReadGenericRepository<Achievement>, IReadAchievementRepository
    {
        public ReadAchievementRepository(GrowDayDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Achievement>> GetAllAchievementsAsync(int pageIndex = 0, int pageSize = 10)
        {
            return await _table
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}
