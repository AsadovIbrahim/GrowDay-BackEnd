using GrowDay.Application.Repositories;
using GrowDay.Application.Services;
using GrowDay.Domain.DTO;
using GrowDay.Domain.Entities.Concretes;
using GrowDay.Domain.Enums;
using GrowDay.Domain.Helpers;
using Microsoft.Extensions.Logging;

namespace GrowDay.Persistance.Services
{
    public class UserHabitService : IUserHabitService
    {
        protected readonly IWriteUserHabitRepository _writeUserHabitRepository;
        protected readonly IWriteHabitRepository _writeHabitRepository;
        protected readonly IReadHabitRepository _readHabitRepository;
        protected readonly IReadUserHabitRepository _readUserHabitRepository;
        protected readonly IWriteNotificationRepository _writeNotificationRepository;
        protected readonly IReadHabitRecordRepository _readHabitRecordRepository;
        protected readonly IWriteHabitRecordRepository _writeHabitRecordRepository;
        protected readonly IReadSuggestedHabitRepository _readSuggestedHabitRepository;
        protected readonly IWriteTaskRepository _writeTaskRepository;
        protected readonly IReadTaskRepository _readTaskRepository;
        protected readonly IWriteUserTaskRepository _writeUserTaskRepository;
        protected readonly IWriteSuggestedHabitRepository _writeSuggestedHabitRepository;
        protected readonly IReadUserTaskRepository _readUserTaskRepository;
        protected readonly IReadUserTaskCompletionRepository _readUserTaskCompletionRepository;
        protected readonly IWriteUserTaskCompletionRepository _writeUserTaskCompletionRepository;
        protected readonly INotificationService _notificationService;


        protected readonly ILogger<UserHabitService> _logger;

        public UserHabitService(IWriteUserHabitRepository userHabitRepository, ILogger<UserHabitService> logger, IReadUserHabitRepository readUserHabitRepository,
            IWriteHabitRepository writeHabitRepository, IReadHabitRepository readHabitRepository, INotificationService notificationService,
            IReadSuggestedHabitRepository readSuggestedHabitRepository, IWriteNotificationRepository writeNotificationRepository,
            IReadHabitRecordRepository readHabitRecordRepository, IWriteHabitRecordRepository writeHabitRecordRepository, IWriteTaskRepository writeTaskRepository,
            IReadTaskRepository readTaskRepository, IWriteUserTaskRepository writeUserTaskRepository, IWriteSuggestedHabitRepository writeSuggestedHabitRepository,
            IReadUserTaskCompletionRepository readUserTaskCompletionRepository, IWriteUserTaskCompletionRepository writeUserTaskCompletionRepository, 
            IReadUserTaskRepository readUserTaskRepository)
        {
            _writeUserHabitRepository = userHabitRepository;
            _logger = logger;
            _readUserHabitRepository = readUserHabitRepository;
            _writeHabitRepository = writeHabitRepository;
            _readHabitRepository = readHabitRepository;
            _notificationService = notificationService;
            _readSuggestedHabitRepository = readSuggestedHabitRepository;
            _writeNotificationRepository = writeNotificationRepository;
            _readHabitRecordRepository = readHabitRecordRepository;
            _writeHabitRecordRepository = writeHabitRecordRepository;
            _writeTaskRepository = writeTaskRepository;
            _readTaskRepository = readTaskRepository;
            _writeUserTaskRepository = writeUserTaskRepository;
            _writeSuggestedHabitRepository = writeSuggestedHabitRepository;
            _readUserTaskCompletionRepository = readUserTaskCompletionRepository;
            _writeUserTaskCompletionRepository = writeUserTaskCompletionRepository;
            _readUserTaskRepository = readUserTaskRepository;
        }

        public async Task<Result> AddFromSuggestedHabitAsync(string userId, AddSuggestedHabitDTO addSuggestedHabitDTO)
        {
            try
            {
                var suggestedHabit = await _readSuggestedHabitRepository.GetByIdAsync(addSuggestedHabitDTO.SuggestedHabitId);
                if (suggestedHabit == null)
                    return Result.FailureResult("Suggested habit not found.");

                var existingUserHabit = await _readUserHabitRepository.GetByUserAndHabitAsync(userId, suggestedHabit.Title);
                if (existingUserHabit != null)
                    return Result.FailureResult("User habit already exists.");

                var userHabit = new UserHabit
                {
                    UserId = userId,
                    HabitId = null,
                    Title = suggestedHabit.Title,
                    Description = suggestedHabit.Description,
                    Frequency = suggestedHabit.Frequency,
                    CreatedAt = DateTime.UtcNow,
                    CurrentStreak = 0,
                    LongestStreak = 0,
                    StartDate = addSuggestedHabitDTO.StartDate ?? DateTime.UtcNow,
                    EndDate = addSuggestedHabitDTO.EndDate ?? suggestedHabit.EndDate,
                    NotificationTime = addSuggestedHabitDTO.NotificationTime ?? suggestedHabit.NotificationTime,
                    DurationInMinutes = addSuggestedHabitDTO.DurationInMinutes ?? suggestedHabit.DurationInMinutes,
                    LastCompletedDate = null,
                    CurrentValue = 0,
                    TargetValue = suggestedHabit.TargetValue,
                    IncrementValue = suggestedHabit.IncrementValue,
                    Unit = suggestedHabit.Unit,
                    IsActive = true,
                    IsDeleted = false
                };
                await _writeUserHabitRepository.AddAsync(userHabit);

                return Result.SuccessResult("Habit added from suggested habit successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding habit from suggested habit");
                return Result.FailureResult("An error occurred while adding the habit from suggested habit.");
            }
        }

        public async Task<Result> AddUserHabitAsync(string userId, AddUserHabitDTO dto)
        {
            try
            {
                if (string.IsNullOrEmpty(dto.HabitId))
                    return Result.FailureResult("HabitId is required.");

                var habit = await _readHabitRepository.GetByIdAsync(dto.HabitId);
                if (habit == null)
                    return Result.FailureResult("Habit not found.");
                var existingUserHabit = await _readUserHabitRepository.GetByUserAndHabitAsync(userId, dto.HabitId);
                if (existingUserHabit != null && !existingUserHabit.IsDeleted)
                    return Result.FailureResult("User habit already exists.");

                var userHabitFrequency = dto.Frequency ?? habit.Frequency;

                var userHabit = new UserHabit
                {
                    UserId = userId,
                    HabitId = dto.HabitId,
                    CreatedAt = DateTime.UtcNow,
                    CurrentStreak = 0,
                    LongestStreak = 0,
                    NotificationTime = dto.NotificationTime,
                    DurationInMinutes = dto.DurationInMinutes,
                    StartDate = dto.StartDate ?? DateTime.UtcNow,
                    EndDate = dto.EndDate ?? habit.EndDate,
                    CurrentValue = 0,
                    TargetValue = habit.TargetValue,
                    IncrementValue = habit.IncrementValue,
                    Unit = habit.Unit,
                    Frequency = userHabitFrequency,
                    LastCompletedDate = null,
                    IsActive = true,
                    IsDeleted = false
                };

                await _writeUserHabitRepository.AddAsync(userHabit);
                var habitTasks = await _readTaskRepository.GetByHabitIdAsync(dto.HabitId);
                foreach (var task in habitTasks)
                {
                    var userTask = new UserTask
                    {
                        TaskId = task.Id,
                        UserId = userId,
                        UserHabitId = userHabit.Id,
                        Title = task.Title,
                        Description = task.Description,
                        Points = task.Points,
                        TotalRequiredCompletions = task.TotalRequiredCompletions,
                        RequiredPoints = task.RequiredPoints,
                        IsCompleted = false,
                        CreatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    };
                    await _writeUserTaskRepository.AddAsync(userTask);
                }
                return Result.SuccessResult($"User habit added successfully. UserHabitId: {userHabit.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding admin habit to user");
                return Result.FailureResult("An error occurred while adding the habit.");
            }
        }

        public async Task<Result> AddUserOwnHabitAsync(string userId, AddUserOwnHabitDTO addUserOwnHabitDTO)
        {
            try
            {
                var existingUserHabit = await _readUserHabitRepository.GetByUserAndHabitAsync(userId, addUserOwnHabitDTO.Title);
                if (existingUserHabit != null)
                    return Result.FailureResult("User habit with the same title already exists.");

                var userHabit = new UserHabit
                {
                    UserId = userId,
                    HabitId = null,
                    Title = addUserOwnHabitDTO.Title,
                    Description = addUserOwnHabitDTO.Description,
                    Frequency = addUserOwnHabitDTO.Frequency,
                    CreatedAt = DateTime.UtcNow,
                    CurrentStreak = 0,
                    LongestStreak = 0,
                    StartDate = DateTime.UtcNow,
                    EndDate = addUserOwnHabitDTO.EndDate,
                    CurrentValue = 0,
                    TargetValue = addUserOwnHabitDTO.TargetValue,
                    IncrementValue = addUserOwnHabitDTO.IncrementValue,
                    Unit = addUserOwnHabitDTO.Unit,
                    NotificationTime = addUserOwnHabitDTO.NotificationTime,
                    DurationInMinutes = addUserOwnHabitDTO.DurationInMinutes,
                    LastCompletedDate = null,
                    IsActive = true,
                    IsDeleted = false
                };
                await _writeUserHabitRepository.AddAsync(userHabit);
                return Result.SuccessResult($"User habit added successfully. Id: {userHabit.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding user habit");
                return Result.FailureResult("An error occurred while adding the user habit.");
            }

        }

        public async Task<Result> ClearUserHabitsAsync(string userId)
        {
            try
            {
                var userHabits = await _readUserHabitRepository.GetByUserIdAsync(userId);
                if (userHabits == null || !userHabits.Any())
                {
                    return Result.FailureResult("No habits found for the user.");
                }
                foreach (var habit in userHabits)
                {
                    var userTasks = await _readUserTaskRepository.GetTasksByHabitIdAsync(userId, habit.Id);
                    if (userTasks != null && userTasks.Any())
                        await _writeUserTaskRepository.RemoveRangeAsync(userTasks);
                    await _writeUserHabitRepository.DeleteAsync(habit);
                }
                return Result.SuccessResult("User habits cleared successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing user habits");
                return Result.FailureResult("An error occurred while clearing user habits.");
            }
        }

        public async Task<Result<UserHabitDTO>> CompleteHabitAsync(string userId, string userHabitId, string? note = null)
        {
            try
            {
                var userHabit = await _readUserHabitRepository.GetByUserAndHabitAsync(userId, userHabitId);
                if (userHabit == null)
                    return Result<UserHabitDTO>.FailureResult("User habit not found.");

                var today = DateTime.UtcNow.Date;

                var existingHabitRecord = await _readHabitRecordRepository.GetByUserHabitIdAndDateAsync(userHabit.Id, today);

                if (existingHabitRecord != null && existingHabitRecord.IsCompleted && !existingHabitRecord.IsDeleted)
                    return Result<UserHabitDTO>.FailureResult("Habit already completed for today.");

                if (userHabit.IncrementValue.HasValue && userHabit.TargetValue.HasValue)
                {
                    userHabit.CurrentValue += userHabit.IncrementValue.Value;

                    if (userHabit.CurrentValue >= userHabit.TargetValue.Value)
                    {
                        userHabit.CurrentValue = 0; // növbəti dövr üçün sıfırla
                        userHabit.LastCompletedDate = today;

                        if (userHabit.LastCompletedDate.HasValue && userHabit.LastCompletedDate.Value.Date == today.AddDays(-1))
                            userHabit.CurrentStreak++;
                        else
                            userHabit.CurrentStreak = 1;

                        if (userHabit.CurrentStreak > userHabit.LongestStreak)
                            userHabit.LongestStreak = userHabit.CurrentStreak;


                        if(existingHabitRecord!=null && !existingHabitRecord.IsDeleted)
                        {
                            existingHabitRecord.IsCompleted = true;
                            if (!string.IsNullOrEmpty(note))
                                existingHabitRecord.Note = note;
                            existingHabitRecord.LastModifiedAt = DateTime.UtcNow;
                            await _writeHabitRecordRepository.UpdateAsync(existingHabitRecord);
                        }
                        else
                        {
                            var habitRecord = new HabitRecord
                            {
                                UserHabitId = userHabit.Id,
                                Date = today,
                                Note = note,
                                CreatedAt = DateTime.UtcNow,
                                LastModifiedAt = DateTime.UtcNow,
                                IsCompleted = true,
                                IsDeleted = false
                            };

                            await _writeHabitRecordRepository.AddAsync(habitRecord);
                        }


                        await _notificationService.CreateAndSendNotificationAsync(
                            userHabit.Id,
                            userId,
                            "Habit Completed ✅",
                            $"You reached your goal for {(!string.IsNullOrEmpty(userHabit.Title) ? userHabit.Title : userHabit.Habit?.Title)}! Great job!",
                            NotificationType.Achievement
                        );
                    }
                    else
                    {
                        if (existingHabitRecord == null || existingHabitRecord.IsDeleted)
                        {
                            var habitRecord = new HabitRecord
                            {
                                UserHabitId = userHabit.Id,
                                Date = today,
                                Note = note,
                                CreatedAt = DateTime.UtcNow,
                                LastModifiedAt = DateTime.UtcNow,
                                IsCompleted = false,
                                IsDeleted = false
                            };
                            await _writeHabitRecordRepository.AddAsync(habitRecord);
                        }
                        else
                        {
                            existingHabitRecord.IsCompleted = false;
                            if (!string.IsNullOrEmpty(note))
                                existingHabitRecord.Note = note;
                            existingHabitRecord.LastModifiedAt = DateTime.UtcNow;
                            await _writeHabitRecordRepository.UpdateAsync(existingHabitRecord);
                        }
                    }
                }

                var relatedUserTasks = await _readUserTaskRepository.GetTasksByHabitIdAsync(userId, userHabit.Id);
                foreach (var userTask in relatedUserTasks)
                    await CompleteTaskProgressAsync(userId, userTask);

                await _writeUserHabitRepository.UpdateAsync(userHabit);

                var habitDTO = new UserHabitDTO
                {
                    UserHabitId = userHabit.Id,
                    UserId = userHabit.UserId,
                    HabitId = userHabit.HabitId,
                    Title = !string.IsNullOrEmpty(userHabit.Title) ? userHabit.Title : userHabit.Habit?.Title,
                    Description = !string.IsNullOrEmpty(userHabit.Description) ? userHabit.Description : userHabit.Habit?.Description,
                    IsActive = !userHabit.IsDeleted,
                    Frequency = userHabit.Frequency ?? userHabit.Habit?.Frequency,
                    StartDate = userHabit.CreatedAt,
                    EndDate = userHabit.EndDate,
                    CurrentValue = userHabit.CurrentValue,
                    TargetValue = userHabit.TargetValue,
                    IncrementValue = userHabit.IncrementValue,
                    Unit = userHabit.Unit,
                    CurrentStreak = userHabit.CurrentStreak,
                    LongestStreak = userHabit.LongestStreak,
                    NotificationTime = userHabit.NotificationTime,
                    DurationInMinutes = userHabit.DurationInMinutes,
                    LastCompletedDate = userHabit.LastCompletedDate
                };

                return Result<UserHabitDTO>.SuccessResult(habitDTO, "Habit progress updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error completing habit");
                return Result<UserHabitDTO>.FailureResult("An error occurred while completing the habit.");
            }
        }


        private async Task CompleteTaskProgressAsync(string userId, UserTask userTask)
        {
            var today = DateTime.UtcNow.Date;

            var todaysCompletions = await _readUserTaskCompletionRepository.GetUserTaskCompletions(userId, userTask.Id);
            if (todaysCompletions.Any(c => c.CompletedAt.Date == today))
                return;

            var completion = new UserTaskCompletion
            {
                UserTaskId = userTask.Id,
                Points = userTask.Points,
                CompletedAt = DateTime.UtcNow
            };
            await _writeUserTaskCompletionRepository.AddAsync(completion);
        }
        public async Task<Result<ICollection<HabitRecordDTO>>> GetAllCompletedHabitsAsync(string userId)
        {
            try
            {
                var userHabits = await _readHabitRecordRepository.GetAllByUserAsync(userId);
                if (userHabits == null || !userHabits.Any())
                {
                    return Result<ICollection<HabitRecordDTO>>.FailureResult("No completed habits found.");
                }
                var habitRecords = userHabits.Where(hr => hr.IsCompleted && !hr.IsDeleted).Select(hr => new HabitRecordDTO
                {
                    Id = hr.Id,
                    UserHabitId = hr.UserHabitId,
                    Date = hr.Date,
                    Note = hr.Note!,
                    IsCompleted = hr.IsCompleted
                }).ToList();

                return Result<ICollection<HabitRecordDTO>>.SuccessResult(habitRecords, "Completed habits retrieved successfully.");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving completed habits");
                return Result<ICollection<HabitRecordDTO>>.FailureResult("An error occurred while retrieving completed habits.");
            }
        }

        public async Task<Result<List<UserHabitDTO>>> GetAllUserHabitAsync()
        {
            try
            {
                var userHabits = await _readUserHabitRepository.GetAllAsync();
                if (userHabits == null || !userHabits.Any())
                {
                    return Result<List<UserHabitDTO>>.FailureResult("No user habits found.");
                }
                var habitList = userHabits.Select(uh => new UserHabitDTO
                {
                    UserHabitId = uh.Id,
                    UserId = uh.UserId,
                    HabitId = uh.HabitId,
                    Title = !string.IsNullOrEmpty(uh.Title) ? uh.Title : uh.Habit?.Title,
                    Description = !string.IsNullOrEmpty(uh.Description) ? uh.Description : uh.Habit?.Description,
                    Frequency = uh.Frequency ?? uh.Habit?.Frequency,
                    IsActive = !uh.IsDeleted,
                    StartDate = uh.CreatedAt,
                    EndDate = uh.EndDate,
                    CurrentValue = uh.CurrentValue,
                    TargetValue = uh.TargetValue,
                    IncrementValue = uh.IncrementValue,
                    Unit = uh.Unit,
                    CurrentStreak = uh.CurrentStreak,
                    LongestStreak = uh.LongestStreak,
                    NotificationTime = uh.NotificationTime,
                    DurationInMinutes = uh.DurationInMinutes,
                    LastCompletedDate = uh.LastCompletedDate,
                }).ToList();
                return Result<List<UserHabitDTO>>.SuccessResult(habitList, "All user habits retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all user habits");
                return Result<List<UserHabitDTO>>.FailureResult("An error occurred while retrieving all user habits.");
            }
        }

        public async Task<Result<UserHabitDTO>> GetUserHabitByIdAsync(string userId, string userHabitId)
        {
            try
            {
                var userHabit = await _readUserHabitRepository.GetByUserAndHabitAsync(userId, userHabitId);
                if (userHabit == null || userHabit.UserId != userId)
                {
                    return Result<UserHabitDTO>.FailureResult("User habit not found.");
                }
                var habitDTO = new UserHabitDTO
                {
                    UserHabitId = userHabit.Id,
                    UserId = userHabit.UserId,
                    HabitId = userHabit.HabitId,
                    Title = !string.IsNullOrEmpty(userHabit.Title) ? userHabit.Title : userHabit.Habit?.Title,
                    Description = !string.IsNullOrEmpty(userHabit.Description) ? userHabit.Description : userHabit.Habit?.Description,
                    Frequency = userHabit.Frequency ?? userHabit.Habit?.Frequency,
                    IsActive = !userHabit.IsDeleted,
                    StartDate = userHabit.CreatedAt,
                    EndDate = userHabit.EndDate,
                    CurrentValue = userHabit.CurrentValue,
                    TargetValue = userHabit.TargetValue,
                    IncrementValue = userHabit.IncrementValue,
                    Unit = userHabit.Unit,
                    CurrentStreak = userHabit.CurrentStreak,
                    LongestStreak = userHabit.LongestStreak,
                    NotificationTime = userHabit.NotificationTime,
                    DurationInMinutes = userHabit.DurationInMinutes,
                    LastCompletedDate = userHabit.LastCompletedDate
                };
                return Result<UserHabitDTO>.SuccessResult(habitDTO, "User habit retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user habit by ID");
                return Result<UserHabitDTO>.FailureResult("An error occurred while retrieving the user habit.");
            }
        }

        public async Task<Result<bool>> IsHabitCompletedTodayAsync(string userId, string userHabitId, DateTime date)
        {
            try
        
            {

                var userHabit = await _readUserHabitRepository.GetByUserAndHabitAsync(userId, userHabitId);
                if (userHabit == null)
                    return Result<bool>.FailureResult("User habit not found.");
                var habitRecord = await _readHabitRecordRepository.GetByUserHabitIdAndDateAsync(userHabit.Id, date.Date);
                if (habitRecord != null && habitRecord.IsCompleted && !habitRecord.IsDeleted)
                {
                    return Result<bool>.SuccessResult(true, "Habit is completed for the specified date.");
                }
                return Result<bool>.SuccessResult(false, "Habit is not completed for the specified date.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if habit is completed today");
                return Result<bool>.FailureResult("An error occurred while checking if the habit is completed.");
            }

        }

        public async Task<Result<bool>> IsUserHabitExistsAsync(string userId, string habitId)
        {
            try
            {
                var userHabit = await _readUserHabitRepository.GetByUserAndHabitAsync(userId, habitId);
                if (userHabit != null)
                {
                    return Result<bool>.SuccessResult(true, "User habit exists.");
                }
                return Result<bool>.SuccessResult(false, "User habit does not exist.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if user habit exists");
                return Result<bool>.FailureResult("An error occurred while checking if user habit exists.");
            }
        }
        public async Task<Result> RemoveUserHabitAsync(string userId, string userHabitId)
        {
            try
            {
                var userHabit = await _readUserHabitRepository.GetByUserAndHabitAsync(userId, userHabitId);
                if (userHabit == null || userHabit.UserId != userId)
                    return Result.FailureResult("User habit not found.");

                if (userHabit.UserTasks != null && userHabit.UserTasks.Any())
                    await _writeUserTaskRepository.RemoveRangeAsync(userHabit.UserTasks);

                if (userHabit.HabitRecords != null && userHabit.HabitRecords.Any())
                    await _writeHabitRecordRepository.RemoveRangeAsync(userHabit.HabitRecords);

                if (userHabit.Notifications != null && userHabit.Notifications.Any())
                    await _writeNotificationRepository.RemoveRangeAsync(userHabit.Notifications);

                if (userHabit.SuggestedHabits != null && userHabit.SuggestedHabits.Any())
                    await _writeSuggestedHabitRepository.RemoveRangeAsync(userHabit.SuggestedHabits);

                await _writeUserHabitRepository.DeleteAsync(userHabit);

                return Result.SuccessResult("User habit removed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error removing user habit: {ex.Message} | {ex.StackTrace}");
                return Result.FailureResult("An error occurred while removing the user habit.");
            }
        }

        public async Task<Result<UserHabitDTO>> UpdateUserHabitAsync(string userId, string userHabitId, UpdateUserHabitDTO updateUserHabitDTO)
        {
            try
            {
                var userHabit = await _readUserHabitRepository.GetByUserAndHabitAsync(userId, userHabitId);
                if (userHabit == null || userHabit.UserId != userId)
                {
                    return Result<UserHabitDTO>.FailureResult("User habit not found.");
                }
                if (!string.IsNullOrEmpty(updateUserHabitDTO.Title))
                {
                    userHabit.Title = updateUserHabitDTO.Title;
                }
                if (!string.IsNullOrEmpty(updateUserHabitDTO.Description))
                {
                    userHabit.Description = updateUserHabitDTO.Description;
                }
                if (updateUserHabitDTO.Frequency.HasValue)
                {
                    userHabit.Frequency = updateUserHabitDTO.Frequency;
                }
                await _writeUserHabitRepository.UpdateAsync(userHabit);
                var habitDTO = new UserHabitDTO
                {
                    UserHabitId = userHabit.Id,
                    UserId = userHabit.UserId,
                    HabitId = userHabit.HabitId,
                    Title = !string.IsNullOrEmpty(userHabit.Title) ? userHabit.Title : userHabit.Habit?.Title,
                    Description = !string.IsNullOrEmpty(userHabit.Description) ? userHabit.Description : userHabit.Habit?.Description,
                    Frequency = userHabit.Frequency ?? userHabit.Habit?.Frequency,
                    IsActive = !userHabit.IsDeleted,
                    StartDate = userHabit.CreatedAt,
                    EndDate = userHabit.EndDate,
                    CurrentValue = userHabit.CurrentValue,
                    TargetValue = userHabit.TargetValue,
                    IncrementValue = userHabit.IncrementValue,
                    Unit = userHabit.Unit,
                    CurrentStreak = userHabit.CurrentStreak,
                    LongestStreak = userHabit.LongestStreak,
                    NotificationTime = userHabit.NotificationTime,
                    DurationInMinutes = userHabit.DurationInMinutes,
                    LastCompletedDate = userHabit.LastCompletedDate
                };
                return Result<UserHabitDTO>.SuccessResult(habitDTO, "User habit updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user habit");
                return Result<UserHabitDTO>.FailureResult("An error occurred while updating the user habit.");
            }
        }

        public async Task<Result<ICollection<WeeklyHabitProgressDTO>>> GetWeeklyHabitProgressAsync(string userId,string userHabitId)
        {
            try
            {   
                var userHabit = await _readUserHabitRepository.GetByUserAndHabitAsync(userId, userHabitId);
                if (userHabit == null || userHabit.UserId != userId)
                {
                    return Result<ICollection<WeeklyHabitProgressDTO>>.FailureResult("User habit not found.");
                }
                var last7Days = Enumerable.Range(0, 7)
                    .Select(i => DateTime.UtcNow.Date.AddDays(-i))
                    .OrderBy(d => d)
                    .ToList();
                var progressList = new List<WeeklyHabitProgressDTO>();
                foreach (var date in last7Days)
                {
                    var habitRecord = await _readHabitRecordRepository.GetByUserHabitIdAndDateAsync(userHabitId, date);
                    progressList.Add(new WeeklyHabitProgressDTO
                    {
                        Date = date,
                        Day = date.DayOfWeek.ToString(),
                        IsCompleted = habitRecord != null && habitRecord.IsCompleted && !habitRecord.IsDeleted
                    });
                }
                return Result<ICollection<WeeklyHabitProgressDTO>>.SuccessResult(progressList, "Weekly habit progress retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving weekly habit progress");
                return Result<ICollection<WeeklyHabitProgressDTO>>.FailureResult("An error occurred while retrieving weekly habit progress.");
            }
        }
    }
}
