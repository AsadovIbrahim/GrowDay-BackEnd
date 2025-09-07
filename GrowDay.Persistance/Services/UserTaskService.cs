using GrowDay.Domain.DTO;
using GrowDay.Domain.Helpers;
using GrowDay.Application.Services;
using Microsoft.Extensions.Logging;
using GrowDay.Application.Repositories;
using GrowDay.Domain.Entities.Concretes;
using GrowDay.Domain.Enums;

namespace GrowDay.Persistance.Services
{
    public class UserTaskService : IUserTaskService
    {
        protected readonly IWriteUserTaskRepository _writeUserTaskRepository;
        protected readonly IReadAchievementRepository _readAchievementRepository;
        protected readonly IWriteUserAchievementRepository _writeUserAchievementRepository;
        protected readonly IReadUserAchievementRepository _readUserAchievementRepository;
        protected readonly IReadUserTaskRepository _readUserTaskRepository;
        protected readonly IReadUserHabitRepository _readUserHabitRepository;
        protected readonly IStatisticService _statisticService;
        protected readonly INotificationService _notificationService;
        protected readonly ILogger<UserTaskService> _logger;

        public UserTaskService(IWriteUserTaskRepository writeUserTaskRepository, IReadUserTaskRepository readUserTaskRepository, ILogger<UserTaskService> logger,
            IReadAchievementRepository readAchievementRepository, IWriteUserAchievementRepository writeUserAchievementRepository,
            IReadUserAchievementRepository readUserAchievementRepository, IStatisticService statisticService, IReadUserHabitRepository readUserHabitRepository,
            INotificationService notificationService)
        {
            _writeUserTaskRepository = writeUserTaskRepository;
            _readUserTaskRepository = readUserTaskRepository;
            _logger = logger;
            _readAchievementRepository = readAchievementRepository;
            _writeUserAchievementRepository = writeUserAchievementRepository;
            _readUserAchievementRepository = readUserAchievementRepository;
            _statisticService = statisticService;
            _readUserHabitRepository = readUserHabitRepository;
            _notificationService = notificationService;
        }

        public async Task<Result<UserTaskDTO>> CompleteTaskAsync(string userId, string taskId)
        {
            try
            {
                var userTaskResult = await _readUserTaskRepository.GetUserTaskByIdAsync(userId, taskId);
                if (userTaskResult == null)
                {
                    return Result<UserTaskDTO>.FailureResult("Task not found.");
                }
                var today = DateTime.UtcNow.Date;
                if (userTaskResult.IsCompleted && userTaskResult.CompletedAt.HasValue && userTaskResult.CompletedAt.Value.Date == today)
                {
                    return Result<UserTaskDTO>.FailureResult("Task has already been completed today.");
                }
                userTaskResult.IsCompleted = true;
                userTaskResult.CompletedAt = DateTime.UtcNow;
                await _writeUserTaskRepository.UpdateAsync(userTaskResult);

                await CheckAndGrantAchievementsAsync(userId);

                var userTaskDTO = new UserTaskDTO
                {
                    UserTaskId = userTaskResult.Id,
                    Title = userTaskResult.Title,
                    Description = userTaskResult.Description,
                    Points = userTaskResult.Points,
                    IsCompleted = userTaskResult.IsCompleted,
                    CompletedAt = userTaskResult.CompletedAt
                };
                return Result<UserTaskDTO>.SuccessResult(userTaskDTO, "Task completed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error completing task {TaskId} for user {UserId}", taskId, userId);
                return Result<UserTaskDTO>.FailureResult("An error occurred while completing the task.");
            }
        }

        public async Task<Result<IEnumerable<UserTaskDTO>>> GetAllTasksAsync(string userId)
        {
            try
            {
                var userTasks = await _readUserTaskRepository.GetUserTasksByUserIdAsync(userId);
                if (!userTasks.Any())
                {
                    return Result<IEnumerable<UserTaskDTO>>
                        .SuccessResult(Enumerable.Empty<UserTaskDTO>(), "No tasks found for the user.");
                }
                var userTaskDTOs = userTasks.Select(ut => new UserTaskDTO
                {
                    UserTaskId = ut.Id,
                    Title = ut.Title,
                    Description = ut.Description,
                    Points = ut.Points,
                    IsCompleted = ut.IsCompleted,
                    CompletedAt = ut.CompletedAt,
                }).ToList();
                return Result<IEnumerable<UserTaskDTO>>.SuccessResult(userTaskDTOs, "User tasks retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving tasks for user {UserId}", userId);
                return Result<IEnumerable<UserTaskDTO>>.FailureResult("An error occurred while retrieving tasks.");
            }
        }

        public async Task CheckAndGrantAchievementsAsync(string userId)
        {
            var achievements = await _readAchievementRepository.GetAllAsync();

            var userHabits = await _readUserHabitRepository.GetByUserIdAsync(userId);
            var totalCompletedTasksAllTime = await _readUserTaskRepository.GetCompletedUserTaskCountByUserIdAsync(userId);
            var longestStreak = userHabits.Max(h => h.LongestStreak);

            foreach (var achievement in achievements!)
            {
                bool meetsRequirement = true;

                if (achievement.StreakRequired.HasValue && longestStreak < achievement.StreakRequired.Value)
                    meetsRequirement = false;

                if (achievement.TaskCompletionRequired.HasValue && totalCompletedTasksAllTime < achievement.TaskCompletionRequired.Value)
                    meetsRequirement = false;

                if (achievement.PointsRequired > 0)
                {
                    var completedTasks = await _readUserTaskRepository.GetUserTasksByUserIdAsync(userId);
                    var totalPoints = completedTasks.Where(t => t.IsCompleted).Sum(t => t.Points);
                    if (totalPoints < achievement.PointsRequired)
                        meetsRequirement = false;
                }

                if (meetsRequirement)
                {
                    bool alreadyHas = await _readUserAchievementRepository.HasUserAchievementAsync(userId, achievement.Id);
                    if (!alreadyHas)
                    {
                        var userAchievement = new UserAchievement
                        {
                            UserId = userId,
                            AchievementId = achievement.Id,
                            EarnedAt = DateTime.UtcNow
                        };
                        await _writeUserAchievementRepository.AddAsync(userAchievement);
                        await _notificationService.CreateAndSendNotificationAsync(
                            null,
                            userId,
                            "New Achievement Unlocked! 🏆",
                            $"Congratulations! You've unlocked the achievement: {achievement.Title}",
                            NotificationType.Achievement);
                    }
                }
            }
        }

        public async Task<Result> DeleteUserTaskAsync(string userTaskId)
        {
            try
            {
                var userTask = await _readUserTaskRepository.GetByIdAsync(userTaskId);
                if (userTask == null)
                {
                    return Result.FailureResult("User task not found.");
                }
                await _writeUserTaskRepository.DeleteAsync(userTask);
                return Result.SuccessResult("User task deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user task {UserTaskId}", userTaskId);
                return Result.FailureResult("An error occurred while deleting the user task.");
            }
        }
    }
}
