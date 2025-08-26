using GrowDay.Application.Repositories;
using GrowDay.Application.Services;
using GrowDay.Domain.DTO;
using GrowDay.Domain.Entities.Concretes;
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
        protected readonly IReadSuggestedHabitRepository _readSuggestedHabitRepository;
        protected readonly INotificationService _notificationService;

        protected readonly ILogger<UserHabitService> _logger;

        public UserHabitService(IWriteUserHabitRepository userHabitRepository, ILogger<UserHabitService> logger, IReadUserHabitRepository readUserHabitRepository,
            IWriteHabitRepository writeHabitRepository, IReadHabitRepository readHabitRepository, INotificationService notificationService, 
            IReadSuggestedHabitRepository readSuggestedHabitRepository)
        {
            _writeUserHabitRepository = userHabitRepository;
            _logger = logger;
            _readUserHabitRepository = readUserHabitRepository;
            _writeHabitRepository = writeHabitRepository;
            _readHabitRepository = readHabitRepository;
            _notificationService = notificationService;
            _readSuggestedHabitRepository = readSuggestedHabitRepository;
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
                if (existingUserHabit != null)
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
                    Frequency = userHabitFrequency,
                    LastCompletedDate = null,
                    IsActive = true,
                    IsDeleted = false
                };

                await _writeUserHabitRepository.AddAsync(userHabit);
                if (dto.NotificationTime.HasValue)
                {
                    var notificationResult = await _notificationService.CreateAndSendNotificationAsync(
                        userHabit.Id,
                        userId,
                        habit.Title,
                        "Time to complete it.");
                }
                return Result.SuccessResult("Admin habit added to user successfully.");
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
                foreach (var userHabit in userHabits)
                {
                    userHabit.IsDeleted = true;
                    await _writeUserHabitRepository.DeleteAsync(userHabit);
                }
                return Result.SuccessResult("User habits cleared successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing user habits");
                return Result.FailureResult("An error occurred while clearing user habits.");
            }
        }

        public async Task<Result<UserHabitDTO>> CompleteHabitAsync(string userId, string habitId)
        {
            try
            {
                var userHabit = await _readUserHabitRepository.GetByUserAndHabitAsync(userId, habitId);
                if (userHabit == null)
                {
                    return Result<UserHabitDTO>.FailureResult("User habit not found.");
                }

                var today = DateTime.UtcNow.Date;

                if (userHabit.LastCompletedDate.HasValue && userHabit.LastCompletedDate.Value.Date == today)
                {
                    return Result<UserHabitDTO>.FailureResult("Habit already completed for today.");
                }

                if (userHabit.LastCompletedDate.HasValue && userHabit.LastCompletedDate.Value.Date == today.AddDays(-1))
                {
                    userHabit.CurrentStreak++;
                }
                else
                {
                    userHabit.CurrentStreak = 1;
                }

                if (userHabit.CurrentStreak > userHabit.LongestStreak)
                    userHabit.LongestStreak = userHabit.CurrentStreak;

                userHabit.LastCompletedDate = today;

                await _writeUserHabitRepository.UpdateAsync(userHabit);

                var habitDTO = new UserHabitDTO
                {
                    UserId = userHabit.UserId,
                    HabitId = !string.IsNullOrEmpty(userHabit.HabitId) ? userHabit.HabitId : userHabit.Id,
                    Title = !string.IsNullOrEmpty(userHabit.Title) ? userHabit.Title : userHabit.Habit?.Title,
                    Description = !string.IsNullOrEmpty(userHabit.Description) ? userHabit.Description : userHabit.Habit?.Description,
                    IsActive = !userHabit.IsDeleted,
                    Frequency = userHabit.Frequency ?? userHabit.Habit?.Frequency,
                    StartDate = userHabit.CreatedAt,
                    EndDate = userHabit.EndDate,
                    CurrentStreak = userHabit.CurrentStreak,
                    LongestStreak = userHabit.LongestStreak,
                    NotificationTime = userHabit.NotificationTime,
                    DurationInMinutes = userHabit.DurationInMinutes,
                    LastCompletedDate = userHabit.LastCompletedDate
                };

                return Result<UserHabitDTO>.SuccessResult(habitDTO, "Habit completed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error completing habit");
                return Result<UserHabitDTO>.FailureResult("An error occurred while completing the habit.");
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
                    UserId = uh.UserId,
                    HabitId = !string.IsNullOrEmpty(uh.HabitId) ? uh.HabitId : uh.Id,
                    Title = !string.IsNullOrEmpty(uh.Title) ? uh.Title : uh.Habit?.Title,
                    Description = !string.IsNullOrEmpty(uh.Description) ? uh.Description : uh.Habit?.Description,
                    Frequency = uh.Frequency ?? uh.Habit?.Frequency,
                    IsActive = !uh.IsDeleted,
                    StartDate = uh.CreatedAt,
                    EndDate = uh.EndDate,
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
                    UserId = userHabit.UserId,
                    HabitId = !string.IsNullOrEmpty(userHabit.HabitId) ? userHabit.HabitId : userHabit.Id,
                    Title = !string.IsNullOrEmpty(userHabit.Title) ? userHabit.Title : userHabit.Habit?.Title,
                    Description = !string.IsNullOrEmpty(userHabit.Description) ? userHabit.Description : userHabit.Habit?.Description,
                    Frequency = userHabit.Frequency ?? userHabit.Habit?.Frequency,
                    IsActive = !userHabit.IsDeleted,
                    StartDate = userHabit.CreatedAt,
                    EndDate = userHabit.EndDate,
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
                {
                    return Result.FailureResult("User habit not found.");
                }
                userHabit.IsDeleted = true;
                await _writeUserHabitRepository.DeleteAsync(userHabit);
                return Result.SuccessResult("User habit removed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing user habit");
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
                    HabitId = !string.IsNullOrEmpty(userHabit.HabitId) ? userHabit.HabitId : userHabit.Id,
                    Title = !string.IsNullOrEmpty(userHabit.Title) ? userHabit.Title : userHabit.Habit?.Title,
                    Description = !string.IsNullOrEmpty(userHabit.Description) ? userHabit.Description : userHabit.Habit?.Description,
                    Frequency = userHabit.Frequency ?? userHabit.Habit?.Frequency,
                    IsActive = !userHabit.IsDeleted,
                    StartDate = userHabit.CreatedAt,
                    EndDate = userHabit.EndDate,
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
    }
}
