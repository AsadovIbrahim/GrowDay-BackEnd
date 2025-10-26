using GrowDay.Application.Repositories;
using GrowDay.Domain.Entities.Concretes;
using GrowDay.Persistance.DbContexts;
using GrowDay.Persistance.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace GrowDay.Persistance.Repositories
{
    public class ReadUserAchievementRepository : ReadGenericRepository<UserAchievement>, IReadUserAchievementRepository
    {
        public ReadUserAchievementRepository(GrowDayDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<UserAchievement>> GetUserAchievementsAsync(string userId, int pageIndex = 0, int pageSize = 10)
        {
            return await _table
                .Include(ua => ua.Achievement)
                .Where(ua => ua.UserId == userId)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<bool> HasUserAchievementAsync(string userId, string achievementId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(achievementId))
            {
                return false;
            }
            return await _table.AnyAsync(ua => ua.UserId == userId && ua.AchievementId == achievementId);
        }
    }
}
