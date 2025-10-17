using GrowDay.Domain.DTO;
using GrowDay.Domain.Helpers;
using Microsoft.Extensions.Logging;
using GrowDay.Application.Services;
using GrowDay.Application.Repositories;
using GrowDay.Domain.Entities.Concretes;

namespace GrowDay.Persistance.Services
{
    public class UserActivityService : IUserActivityService
    {
        protected readonly IWriteUserActivityRepository _writeUserActivityRepository;
        protected readonly IReadUserActivityRepository _readUserActivityRepository;
        protected readonly IReadUserTaskCompletionRepository _readUserTaskCompletionRepository;
        protected readonly ILogger<UserActivityService> _logger;
        public UserActivityService(IWriteUserActivityRepository writeUserActivityRepository, IReadUserActivityRepository readUserActivityRepository,
            ILogger<UserActivityService> logger, IReadUserTaskCompletionRepository readUserTaskCompletionRepository)
        {
            _writeUserActivityRepository = writeUserActivityRepository;
            _readUserActivityRepository = readUserActivityRepository;
            _readUserTaskCompletionRepository = readUserTaskCompletionRepository;
            _logger = logger;
        }
        public async Task<Result> ClearActivityAsync(string userId)
        {
            try
            {
                var activities = await _readUserActivityRepository.GetUserActivitiesAsync(userId);
                if (activities == null || !activities.Any())
                {
                    return Result.FailureResult("No activities found for the user.");
                }
                foreach (var activity in activities)
                {
                    await _writeUserActivityRepository.DeleteAsync(activity);
                }
                return Result.SuccessResult("All user activities cleared successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while clearing user activities.");
                return Result.FailureResult("An error occurred while clearing activities.");
            }
        }
        public async Task<Result<UserActivityDTO>> CreateActivityAsync(CreateActivityDTO createActivityDTO)
        {
            try
            {
                var userActivity = new UserActivity
                {
                    UserId = createActivityDTO.UserId,
                    Title = createActivityDTO.Title,
                    Description = createActivityDTO.Description,
                    ActivityType = createActivityDTO.ActivityType,
                    CreatedAt = DateTime.UtcNow
                };
                await _writeUserActivityRepository.AddAsync(userActivity);

                var userActivityDTO = new UserActivityDTO
                {
                    Id = userActivity.Id,
                    UserId = userActivity.UserId,
                    Title = userActivity.Title,
                    Description = userActivity.Description,
                    ActivityType = userActivity.ActivityType,
                    CreatedAt = userActivity.CreatedAt
                };
                return Result<UserActivityDTO>.SuccessResult(userActivityDTO, "User activity created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating user activity.");
                return Result<UserActivityDTO>.FailureResult("An error occurred while creating the activity.");
            }
        }

        public async Task<Result> DeleteActivityAsync(string userId, string activityId)
        {
            try
            {
                var userActivity = await _readUserActivityRepository.GetByIdAsync(activityId);
                if (userActivity == null || userActivity.UserId != userId)
                {
                    return Result.FailureResult("User activity not found.");
                }
                await _writeUserActivityRepository.DeleteAsync(userActivity);
                return Result.SuccessResult("User activity deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting user activity.");
                return Result.FailureResult("An error occurred while deleting the activity.");
            }
        }

        public async Task<Result<IEnumerable<UserActivityDTO>>> GetUserActivitiesAsync(string userId)
        {
            try
            {
                var activities = await _readUserActivityRepository.GetUserActivitiesAsync(userId);
                if (activities == null || !activities.Any())
                {
                    return Result<IEnumerable<UserActivityDTO>>.FailureResult("No activities found for the user.");
                }
                var activityDTOs = activities.Select(activity => new UserActivityDTO
                {
                    Id = activity.Id,
                    UserId = activity.UserId,
                    Title = activity.Title,
                    Description = activity.Description,
                    ActivityType = activity.ActivityType,
                    CreatedAt = activity.CreatedAt
                }).ToList();
                return Result<IEnumerable<UserActivityDTO>>.SuccessResult(activityDTOs, "User activities retrieved successfully.");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving user activities.");
                return Result<IEnumerable<UserActivityDTO>>.FailureResult("An error occurred while retrieving activities.");
            }
        }

        public async Task<Result<int>> GetUserTotalPointsAsync(string userId)   
        {
            try
            {
                var taskCompletions = await _readUserTaskCompletionRepository.GetUserCompletionsByUserIdAsync(userId);
                int totalPoints = taskCompletions.Sum(utc => utc.Points);
                return Result<int>.SuccessResult(totalPoints, "User total points calculated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while calculating user total points.");
                return Result<int>.FailureResult("An error occurred while calculating total points.");
            }
        }
    }
}
