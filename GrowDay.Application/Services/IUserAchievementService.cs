using GrowDay.Domain.DTO;
using GrowDay.Domain.Helpers;

namespace GrowDay.Application.Services
{
    public interface IUserAchievementService
    {
        Task<Result<IEnumerable<UserAchievementDTO>>>GetUserAchievementsAsync(string userId);
    }
}
