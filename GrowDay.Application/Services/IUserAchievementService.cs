using GrowDay.Domain.DTO;
using GrowDay.Domain.Helpers;

namespace GrowDay.Application.Services
{
    public interface IUserAchievementService
    {
        Task<Result<IEnumerable<UserAchievementDTO>>>GetUserAchievementsAsync(string userId,int pageIndex=0,int pageSize=10);
        Task<Result>DeleteUserAchievementAsync(string userId, string userAchievementId);
        Task<Result> ClearAllUserAchievementsAsync();
    }
}
