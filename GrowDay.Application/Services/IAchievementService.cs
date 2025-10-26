using GrowDay.Domain.DTO;
using GrowDay.Domain.Helpers;

namespace GrowDay.Application.Services
{
    public interface IAchievementService
    {
        Task<Result<IEnumerable<AchievementDTO>>> GetAllAchievementsAsync(int pageIndex = 0, int pageSize = 10);
        Task<Result<AchievementDTO>> CreateAchievementAsync(CreateAchievementDTO createAchievementDTO);
        Task<Result<AchievementDTO>> UpdateAchievementAsync(UpdateAchievementDTO updateAchievementDTO);
        Task<Result> DeleteAchievementAsync(string achievementId);
        Task<Result> ClearAllAchievementsAsync();
    }
}
