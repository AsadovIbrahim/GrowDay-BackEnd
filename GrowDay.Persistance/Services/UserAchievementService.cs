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
        protected readonly IWriteUserAchievementRepository _writeUserAchievementRepository;
        protected readonly ILogger<UserAchievementService> _logger;
        public UserAchievementService(IReadUserAchievementRepository readUserAchievementRepository,
            ILogger<UserAchievementService> logger,
            IWriteUserAchievementRepository writeUserAchievementRepository)
        {
            _readUserAchievementRepository = readUserAchievementRepository;
            _logger = logger;
            _writeUserAchievementRepository = writeUserAchievementRepository;
        }

        public async Task<Result> ClearAllUserAchievementsAsync()
        {
            try
            {
                var userAchievements = await _readUserAchievementRepository.GetAllAsync();
                if (userAchievements == null || !userAchievements.Any())
                {
                    return Result.FailureResult("No user achievements found to clear.");
                }
                foreach (var userAchievement in userAchievements)
                {
                    await _writeUserAchievementRepository.DeleteAsync(userAchievement);
                }
                return Result.SuccessResult("All user achievements cleared successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while clearing all user achievements");
                return Result.FailureResult("An error occurred while clearing user achievements.");
            }
        }

        public async Task<Result> DeleteUserAchievementAsync(string userId, string userAchievementId)
        {
            try
            {
                var userAchievement = await _readUserAchievementRepository.GetByIdAsync(userAchievementId);
                if (userAchievement == null)
                {
                    return Result.FailureResult("User achievement not found.");
                }
                if (userAchievement.UserId != userId)
                {
                    return Result.FailureResult("You do not have permission to delete this achievement.");
                }
                await _writeUserAchievementRepository.DeleteAsync(userAchievement);
                return Result.SuccessResult("User achievement deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user achievement {UserAchievementId} for user {UserId}", userAchievementId, userId);
                return Result.FailureResult("An error occurred while deleting the user achievement.");
            }
        }

        public async Task<Result<IEnumerable<UserAchievementDTO>>> GetUserAchievementsAsync(string userId, int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                var userAchievements = await _readUserAchievementRepository.GetUserAchievementsAsync(userId,pageIndex,pageSize);
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
