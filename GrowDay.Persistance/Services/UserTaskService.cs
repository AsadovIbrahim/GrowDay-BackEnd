using GrowDay.Application.Repositories;
using GrowDay.Application.Services;
using GrowDay.Domain.DTO;
using GrowDay.Domain.Entities.Concretes;
using GrowDay.Domain.Enums;
using GrowDay.Domain.Helpers;
using Microsoft.Extensions.Logging;

namespace GrowDay.Persistance.Services
{
    public class UserTaskService : IUserTaskService
    {
        protected readonly IWriteUserTaskRepository _writeUserTaskRepository;
        protected readonly IReadAchievementRepository _readAchievementRepository;
        protected readonly IWriteUserAchievementRepository _writeUserAchievementRepository;
        protected readonly IReadUserAchievementRepository _readUserAchievementRepository;
        protected readonly IReadUserTaskRepository _readUserTaskRepository;
        protected readonly IWriteUserHabitRepository _writeUserHabitRepository;
        protected readonly IReadUserHabitRepository _readUserHabitRepository;
        protected readonly IStatisticService _statisticService;
        protected readonly INotificationService _notificationService;
        protected readonly IUserHabitService _userHabitService;
        protected readonly IWriteUserTaskCompletionRepository _writeUserTaskCompletionRepository;
        protected readonly IReadUserTaskCompletionRepository _readUserTaskCompletionRepository;
        protected readonly IUserActivityService _userActivityService;
        protected readonly ILogger<UserTaskService> _logger;


        public UserTaskService(IWriteUserTaskRepository writeUserTaskRepository, IReadUserTaskRepository readUserTaskRepository, ILogger<UserTaskService> logger,
            IReadAchievementRepository readAchievementRepository, IWriteUserAchievementRepository writeUserAchievementRepository,
            IReadUserAchievementRepository readUserAchievementRepository, IStatisticService statisticService, IReadUserHabitRepository readUserHabitRepository,
            INotificationService notificationService, IUserHabitService userHabitService, IReadUserTaskCompletionRepository readUserTaskCompletionRepository,
            IWriteUserTaskCompletionRepository writeUserTaskCompletionRepository, IWriteUserHabitRepository writeUserHabitRepository, 
            IUserActivityService userActivityService)
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
            _userHabitService = userHabitService;
            _readUserTaskCompletionRepository = readUserTaskCompletionRepository;
            _writeUserTaskCompletionRepository = writeUserTaskCompletionRepository;
            _writeUserHabitRepository = writeUserHabitRepository;
            _userActivityService = userActivityService;
        }

        public async Task<Result<UserTaskDTO>> CompleteTaskAsync(string userId, string taskId)
        {
            try
            {
                var userTask = await _readUserTaskRepository.GetUserTaskByIdAsync(userId, taskId);
                if (userTask == null)
                    return Result<UserTaskDTO>.FailureResult("Task not found.");

                var completions = await _readUserTaskCompletionRepository.GetUserTaskCompletions(userId, taskId);
                var totalPoints = completions.Sum(c => c.Points);
                var totalCompleted = completions.Count;
                if (userTask.IsCompleted)
                {
                    return Result<UserTaskDTO>.SuccessResult(new UserTaskDTO
                    {
                        UserTaskId = userTask.Id,
                        Title = userTask.Title,
                        Description = userTask.Description,
                        Points = userTask.Points,
                        IsCompleted = userTask.IsCompleted,
                        TotalPointsEarned = totalPoints,
                        TotalTasksCompleted = totalCompleted,
                        CompletedAt = userTask.CompletedAt
                    }, "Task has already been completed.");
                }

                var today = DateTime.UtcNow.Date;
                var todaysCompletions = await _readUserTaskCompletionRepository.GetUserTaskCompletions(userId, taskId);
                if (todaysCompletions.Any(c => c.CompletedAt.Date == today))
                {
                    return Result<UserTaskDTO>.FailureResult("You can only complete this task once per day.");
                }

                var completion = new UserTaskCompletion
                {
                    UserTaskId = userTask.Id,
                    Points = userTask.Points,
                    CompletedAt = DateTime.UtcNow
                };
                await _writeUserTaskCompletionRepository.AddAsync(completion);

                completions = await _readUserTaskCompletionRepository.GetUserTaskCompletions(userId, taskId);
                totalPoints = completions.Sum(c => c.Points);
                totalCompleted = completions.Count;



                bool requirementsMet = true;

                if (userTask.Task.TotalRequiredCompletions.HasValue && totalCompleted < userTask.Task.TotalRequiredCompletions.Value)
                    requirementsMet = false;

                if (userTask.Task.RequiredPoints.HasValue && totalPoints < userTask.Task.RequiredPoints.Value)
                    requirementsMet = false;



                if (requirementsMet)
                {
                    userTask.IsCompleted = true;
                    userTask.CompletedAt = DateTime.UtcNow;
                    await _writeUserTaskRepository.UpdateAsync(userTask);
                    await _userActivityService.CreateActivityAsync(new CreateActivityDTO
                    {
                        UserId = userId,
                        Title = $"Task Completed: {userTask.Title}, {userTask.TotalPointsEarned} points earned",
                        Description = $"You have completed the task '{userTask.Title}' and earned {userTask.Points} points!",
                        ActivityType = ActivityType.TaskCompleted,
                    });
                }
                if (userTask.UserHabitId != null)
                {
                    var userHabit = await _readUserHabitRepository.GetByIdAsync(userTask.UserHabitId);

                    if (userHabit != null)
                    {
                        if (userHabit.IncrementValue.HasValue)
                            userHabit.CurrentValue += userHabit.IncrementValue.Value;

                        if (userHabit.TargetValue.HasValue && userHabit.CurrentValue >= userHabit.TargetValue.Value)
                        {
                            userHabit.CurrentValue = 0;
                            userHabit.LastCompletedDate = DateTime.UtcNow.Date;
                            userHabit.CurrentStreak++;
                            if (userHabit.CurrentStreak > userHabit.LongestStreak)
                                userHabit.LongestStreak = userHabit.CurrentStreak;
                        }

                        await _writeUserHabitRepository.UpdateAsync(userHabit);
                    }
                }

                await CheckAndGrantAchievementsAsync(userId);

                var userTaskDTO = new UserTaskDTO
                {
                    UserTaskId = userTask.Id,
                    Title = userTask.Title,
                    Description = userTask.Description,
                    Points = userTask.Points,
                    IsCompleted = userTask.IsCompleted,
                    CompletedAt = userTask.CompletedAt,
                    TotalRequiredCompletions = userTask.Task.TotalRequiredCompletions,
                    RequiredPoints = userTask.Task.RequiredPoints,
                    TotalPointsEarned = totalPoints,
                    TotalTasksCompleted = totalCompleted
                };

                return Result<UserTaskDTO>.SuccessResult(userTaskDTO, "Task progress updated successfully.");
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

                var userTaskDTOs = new List<UserTaskDTO>();

                foreach (var ut in userTasks)
                {
                    var completions = await _readUserTaskCompletionRepository.GetUserTaskCompletions(userId, ut.Id);
                    var totalPoints = completions.Sum(c => c.Points);
                    var totalCompleted = completions.Count;

                    userTaskDTOs.Add(new UserTaskDTO
                    {
                        UserTaskId = ut.Id,
                        Title = ut.Title,
                        Description = ut.Description,
                        Points = ut.Points,
                        IsCompleted = ut.IsCompleted,
                        CompletedAt = ut.CompletedAt,
                        UserHabitId = ut.UserHabitId,
                        TotalRequiredCompletions = ut.Task.TotalRequiredCompletions,
                        RequiredPoints = ut.Task.RequiredPoints,
                        TotalPointsEarned = totalPoints,
                        TotalTasksCompleted = totalCompleted
                    });
                }

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

                        await _userActivityService.CreateActivityAsync(new CreateActivityDTO
                        {
                            UserId = userId,
                            Title = $"Achievement Unlocked: {achievement.Title}",
                            Description = $"You have unlocked a new achievement: '{achievement.Title}'!",
                            ActivityType = ActivityType.AchievementEarned,
                        });
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

        public async Task<Result<ICollection<UserTaskDTO>>> GetCompletedTasksAsync(string userId)
        {
            try
            {
                var completedTasks = await _readUserTaskRepository.GetCompletedUserTasksByUserIdAsync(userId);
                if (!completedTasks.Any())
                {
                    return Result<ICollection<UserTaskDTO>>
                        .SuccessResult(new List<UserTaskDTO>(), "No completed tasks found for the user.");
                }

                var completedTaskDTOs = new List<UserTaskDTO>();

                foreach (var ut in completedTasks)
                {
                    var completions = await _readUserTaskCompletionRepository.GetUserTaskCompletions(userId, ut.Id);
                    var totalPoints = completions.Sum(c => c.Points);
                    var totalCompleted = completions.Count;

                    completedTaskDTOs.Add(new UserTaskDTO
                    {
                        UserTaskId = ut.Id,
                        Title = ut.Title,
                        Description = ut.Description,
                        Points = ut.Points,
                        IsCompleted = ut.IsCompleted,
                        CompletedAt = ut.CompletedAt,
                        TotalRequiredCompletions = ut.Task.TotalRequiredCompletions,
                        RequiredPoints = ut.Task.RequiredPoints,
                        TotalPointsEarned = totalPoints,
                        TotalTasksCompleted = totalCompleted
                    });
                }

                return Result<ICollection<UserTaskDTO>>.SuccessResult(completedTaskDTOs, "Completed tasks retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving completed tasks for user {UserId}", userId);
                return Result<ICollection<UserTaskDTO>>.FailureResult("An error occurred while retrieving completed tasks.");
            }
        }

        public async Task<Result<UserTaskStatisticDTO>> GetUserTaskStatsAsync(string userId, string taskId)
        {
            try
            {
                var userTask = await _readUserTaskRepository.GetUserTaskByIdAsync(userId, taskId);
                if (userTask == null)
                    return Result<UserTaskStatisticDTO>.FailureResult("Task not found.");

                var taskCompletions = await _readUserTaskCompletionRepository.GetUserTaskCompletions(userId, taskId);

                var frequency = userTask.UserHabit?.Frequency
                                ?? userTask.UserHabit?.Habit?.Frequency
                                ?? HabitFrequency.Daily;

                if (taskCompletions == null || !taskCompletions.Any())
                {
                    return Result<UserTaskStatisticDTO>.SuccessResult(new UserTaskStatisticDTO
                    {
                        UserTaskId = userTask.Id,
                        Title = userTask.Title,
                        Point = userTask.Points,
                        CompletedAt = null,
                        TotalPoints = 0,
                        TotalTasksCompleted = 0,
                        RequiredPoints = userTask.Task.RequiredPoints ?? 0,
                        TotalRequiredCompletions = userTask.Task.TotalRequiredCompletions ?? 0,
                        CurrentStreak = userTask.UserHabit?.CurrentStreak ?? 0,
                        LongestStreak = userTask.UserHabit?.LongestStreak ?? 0,
                        Frequency = frequency,
                        CurrentValue = userTask.UserHabit?.CurrentValue ?? 0,
                        TargettValue = userTask.UserHabit?.TargetValue ?? 0,
                        CompletionPercentage = 0
                    }, "No completions found for the specified task.");
                }

                var latestCompletion = taskCompletions.OrderByDescending(c => c.CompletedAt).First();

                var totalPoints = taskCompletions.Sum(c => c.Points);
                var totalCompleted = taskCompletions.Count;

                var dto = new UserTaskStatisticDTO
                {
                    UserTaskId = latestCompletion.UserTaskId,
                    Title = latestCompletion.UserTask.Title,
                    Point = latestCompletion.Points,
                    CompletedAt = latestCompletion.CompletedAt,
                    TotalPoints = totalPoints,
                    TotalTasksCompleted = totalCompleted,
                    RequiredPoints = latestCompletion.UserTask.Task.RequiredPoints ?? 0,
                    TotalRequiredCompletions = latestCompletion.UserTask.Task.TotalRequiredCompletions ?? 0,
                    CurrentStreak = userTask.UserHabit?.CurrentStreak ?? 0,
                    LongestStreak = userTask.UserHabit?.LongestStreak ?? 0,
                    CurrentValue = userTask.UserHabit?.CurrentValue ?? 0,
                    TargettValue = userTask.UserHabit?.TargetValue ?? 0,
                    Frequency = frequency,
                    CompletionPercentage =
                        latestCompletion.UserTask.Task.TotalRequiredCompletions.HasValue &&
                        latestCompletion.UserTask.Task.TotalRequiredCompletions.Value > 0
                            ? (double)totalCompleted / latestCompletion.UserTask.Task.TotalRequiredCompletions.Value * 100
                            : 100
                };

                return Result<UserTaskStatisticDTO>.SuccessResult(dto, "Task statistics retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving task stats for user {UserId} and task {TaskId}", userId, taskId);
                return Result<UserTaskStatisticDTO>.FailureResult("An error occurred while retrieving task statistics.");
            }
        }





    }
}
