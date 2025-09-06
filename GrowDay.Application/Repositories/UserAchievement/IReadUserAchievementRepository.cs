using GrowDay.Application.Repositories.Common;
using GrowDay.Domain.Entities.Concretes;

namespace GrowDay.Application.Repositories
{
    public interface IReadUserAchievementRepository : IReadGenericRepository<UserAchievement>
    {
        Task<IEnumerable<UserAchievement>> GetUserAchievementsAsync(string userId);
        Task<bool> HasUserAchievementAsync(string userId, string achievementId);

    }
}
