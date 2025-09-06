using GrowDay.Domain.DTO;
using GrowDay.Domain.Helpers;
using Microsoft.Extensions.Logging;
using GrowDay.Application.Services;
using GrowDay.Application.Repositories;
using GrowDay.Domain.Entities.Concretes;

namespace GrowDay.Persistance.Services
{
    public class AchievementService : IAchievementService
    {
        protected readonly IWriteAchievementRepository _writeAchievementRepository;
        protected readonly IReadAchievementRepository _readAchievementRepository;
        protected readonly ILogger<AchievementService> _logger;
        public AchievementService(IWriteAchievementRepository writeAchievementRepository, IReadAchievementRepository readAchievementRepository,
            ILogger<AchievementService> logger)
        {
            _writeAchievementRepository = writeAchievementRepository;
            _readAchievementRepository = readAchievementRepository;
            _logger = logger;
        }

        public async Task<Result> ClearAllAchievementsAsync()
        {
            try
            {
                var achievements= await _readAchievementRepository.GetAllAsync();
                if (achievements == null || !achievements.Any())
                {
                    return Result.FailureResult("No achievements found to clear.");
                }
                foreach (var achievement in achievements)
                {
                    await _writeAchievementRepository.DeleteAsync(achievement);
                }
                return Result.SuccessResult("All achievements cleared successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while clearing all achievements");
                return Result.FailureResult("An error occurred while clearing achievements.");
            }
        }

        public async Task<Result<AchievementDTO>> CreateAchievementAsync(CreateAchievementDTO createAchievementDTO)
        {
            try
            {
                var newAchievement = new Achievement
                {
                    Title = createAchievementDTO.Title,
                    Description = createAchievementDTO.Description,
                    CreatedAt = DateTime.UtcNow,
                    PointsRequired = createAchievementDTO.PointsRequired,
                    StreakRequired = createAchievementDTO.StreakRequired,
                    TaskCompletionRequired = createAchievementDTO.TaskCompletionRequired
                };
                await _writeAchievementRepository.AddAsync(newAchievement);
                var achievementDTO = new AchievementDTO
                {
                    AchievementId = newAchievement.Id,
                    Title = newAchievement.Title,
                    Description = newAchievement.Description,
                    PointsRequired = newAchievement.PointsRequired,
                    StreakRequired = newAchievement.StreakRequired ?? 0,
                    TaskCompletionRequired = newAchievement.TaskCompletionRequired ?? 0
                };
                return Result<AchievementDTO>.SuccessResult(achievementDTO, "Achievement added successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating achievement {Title}", createAchievementDTO.Title);
                return Result<AchievementDTO>.FailureResult("An error occurred while creating the achievement.");
            }
        }

        public async Task<Result> DeleteAchievementAsync(string achievementId)
        {
            try
            {
                var existingAchievement = await _readAchievementRepository.GetByIdAsync(achievementId);
                if (existingAchievement == null)
                {
                    return Result.FailureResult("Achievement not found.");
                }
                await _writeAchievementRepository.DeleteAsync(existingAchievement);
                return Result.SuccessResult("Achievement deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting achievement {AchievementId}", achievementId);
                return Result.FailureResult("An error occurred while deleting the achievement.");
            }
        }

        public async Task<Result<IEnumerable<AchievementDTO>>> GetAllAchievementsAsync()
        {
            try
            {
                var achievements = await _readAchievementRepository.GetAllAsync();
                if (achievements == null || !achievements.Any())
                {
                    return Result<IEnumerable<AchievementDTO>>.FailureResult("No achievements found.");
                }
                var achievementDTOs = achievements.Select(a => new AchievementDTO
                {
                    AchievementId = a.Id,
                    Title = a.Title,
                    Description = a.Description,
                    PointsRequired = a.PointsRequired,
                    StreakRequired = a.StreakRequired ?? 0,
                    TaskCompletionRequired = a.TaskCompletionRequired ?? 0
                }).ToList();
                return Result<IEnumerable<AchievementDTO>>.SuccessResult(achievementDTOs, "Achievements retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all achievements");
                return Result<IEnumerable<AchievementDTO>>.FailureResult("An error occurred while retrieving achievements.");
            }
        }

        public async Task<Result<AchievementDTO>> UpdateAchievementAsync(UpdateAchievementDTO updateAchievementDTO)
        {
            try
            {
                var existingAchievement = await _readAchievementRepository.GetByIdAsync(updateAchievementDTO.AchievementId!);
                if (existingAchievement == null)
                {
                    return Result<AchievementDTO>.FailureResult("Achievement not found.");
                }
                existingAchievement.Title = updateAchievementDTO.Title ?? existingAchievement.Title;
                existingAchievement.Description = updateAchievementDTO.Description ?? existingAchievement.Description;
                existingAchievement.PointsRequired = updateAchievementDTO.PointsRequired != 0 ? updateAchievementDTO.PointsRequired : existingAchievement.PointsRequired;
                existingAchievement.StreakRequired = updateAchievementDTO.StreakRequired != 0 ? updateAchievementDTO.StreakRequired : existingAchievement.StreakRequired;
                existingAchievement.TaskCompletionRequired = updateAchievementDTO.TaskCompletionRequired != 0 ?
                    updateAchievementDTO.TaskCompletionRequired : existingAchievement.TaskCompletionRequired;
                await _writeAchievementRepository.UpdateAsync(existingAchievement);
                var achievementDTO = new AchievementDTO
                {
                    AchievementId = existingAchievement.Id,
                    Title = existingAchievement.Title,
                    Description = existingAchievement.Description,
                    PointsRequired = existingAchievement.PointsRequired,
                    StreakRequired = existingAchievement.StreakRequired ?? 0,
                    TaskCompletionRequired = existingAchievement.TaskCompletionRequired ?? 0
                };
                return Result<AchievementDTO>.SuccessResult(achievementDTO, "Achievement updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating achievement {AchievementId}", updateAchievementDTO.AchievementId);
                return Result<AchievementDTO>.FailureResult("An error occurred while updating the achievement.");
            }
        }
    }
}
