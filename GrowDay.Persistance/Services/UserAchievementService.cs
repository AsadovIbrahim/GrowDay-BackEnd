using GrowDay.Application.Repositories;
using GrowDay.Application.Services;
using GrowDay.Domain.DTO;
using GrowDay.Domain.Helpers;
using Microsoft.Extensions.Logging;

namespace GrowDay.Persistance.Services
{
    public class UserAchievementService : IUserAchievementService
    {
        protected readonly IReadUserAchievementRepository _readUserAchievementRepository;
        protected readonly ILogger<UserAchievementService> _logger;
        public UserAchievementService(IReadUserAchievementRepository readUserAchievementRepository,
            ILogger<UserAchievementService> logger)
        {
            _readUserAchievementRepository = readUserAchievementRepository;
            _logger = logger;
        }
        public async Task<Result<IEnumerable<UserAchievementDTO>>> GetUserAchievementsAsync(string userId)
        {
            try
            {
                var userAchievements = await _readUserAchievementRepository.GetUserAchievementsAsync(userId);
                if (!userAchievements.Any())
                {
                    return Result<IEnumerable<UserAchievementDTO>>
                        .SuccessResult(Enumerable.Empty<UserAchievementDTO>(), "No achievements found for the user.");
                }
                var userAchievementDTOs = userAchievements.Select(ua => new UserAchievementDTO
                {
                    UserAchievementId = ua.Id,
                    AchievementId = ua.AchievementId,
                    Title = ua.Achievement.Title,
                    Description = ua.Achievement.Description,
                    EarnedAt = ua.EarnedAt
                }).ToList();
                return Result<IEnumerable<UserAchievementDTO>>.SuccessResult(userAchievementDTOs, "User achievements retrieved successfully.");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving achievements for user {UserId}", userId);
                return Result<IEnumerable<UserAchievementDTO>>.FailureResult("An error occurred while retrieving user achievements.");
            }
        }
    }
}
