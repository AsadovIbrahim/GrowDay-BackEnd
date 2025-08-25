using GrowDay.Application.Repositories;
using GrowDay.Application.Services;
using GrowDay.Domain.DTO;
using GrowDay.Domain.Entities.Concretes;
using GrowDay.Domain.Helpers;
using Microsoft.Extensions.Logging;

namespace GrowDay.Persistance.Services
{
    public class UserPreferencesService : IUserPreferencesService
    {
        protected readonly IWriteUserPreferencesRepository _writeUserPreferencesRepository;
        protected readonly IReadUserPreferencesRepository _readUserPreferencesRepository;
        protected readonly ILogger<UserPreferencesService> _logger;


        public UserPreferencesService(IWriteUserPreferencesRepository writeUserPreferencesRepository, IReadUserPreferencesRepository readUserPreferencesRepository,
            ILogger<UserPreferencesService> logger)
        {
            _writeUserPreferencesRepository = writeUserPreferencesRepository;
            _readUserPreferencesRepository = readUserPreferencesRepository;
            _logger = logger;
        }
        public async Task<Result<UserPreferenceDTO>> CreateUserPreferencesAsync(string userId, CreateUserPreferenceDTO createUserPreferenceDTO)
        {
            try
            {
                var existingPreferences = await _readUserPreferencesRepository.GetPreferencesByUserIdAsync(userId);
                if (existingPreferences != null)
                {
                    return Result<UserPreferenceDTO>.FailureResult("User preferences already exist.");
                }
                var userPreferences = new UserPreferences
                {
                    UserId = userId,
                    WakeUpTime = createUserPreferenceDTO.WakeUpTime,
                    SleepTime = createUserPreferenceDTO.SleepTime,
                    ProcrestinateFrequency = createUserPreferenceDTO.ProcrestinateFrequency,
                    FocusDifficulty = createUserPreferenceDTO.FocusDifficulty,
                    MotivationalFactors = createUserPreferenceDTO.MotivationalFactors,
                    CreatedAt = DateTime.UtcNow,
                };
                await _writeUserPreferencesRepository.AddAsync(userPreferences);
                var userPreferenceDTO = new UserPreferenceDTO
                {
                    WakeUpTime = userPreferences.WakeUpTime,
                    SleepTime = userPreferences.SleepTime,
                    ProcrestinateFrequency = userPreferences.ProcrestinateFrequency,
                    FocusDifficulty = userPreferences.FocusDifficulty,
                    MotivationalFactors = userPreferences.MotivationalFactors
                };
                return Result<UserPreferenceDTO>.SuccessResult(userPreferenceDTO, "User preferences created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating user preferences for userId: {UserId}", userId);
                return Result<UserPreferenceDTO>.FailureResult("An error occurred while creating user preferences.");
            }
        }

        public async Task<Result<UserPreferenceDTO>> GetUserPreferencesAsync(string userId)
        {
            try
            {
                var userPreferences = await _readUserPreferencesRepository.GetPreferencesByUserIdAsync(userId);
                if (userPreferences == null)
                {
                    return Result<UserPreferenceDTO>.FailureResult("User preferences not found.");
                }
                var userPreferenceDTO = new UserPreferenceDTO
                {
                    WakeUpTime = userPreferences.WakeUpTime,
                    SleepTime = userPreferences.SleepTime,
                    ProcrestinateFrequency = userPreferences.ProcrestinateFrequency,
                    FocusDifficulty = userPreferences.FocusDifficulty,
                    MotivationalFactors = userPreferences.MotivationalFactors
                };

                return Result<UserPreferenceDTO>.SuccessResult(userPreferenceDTO, "User preferences retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving user preferences for userId: {UserId}", userId);
                return Result<UserPreferenceDTO>.FailureResult("An error occurred while retrieving user preferences.");
            }
        }

        public async Task<Result<UserPreferenceDTO>> UpdateUserPreferencesAsync(string userId, UpdateUserPreferenceDTO updateUserPreferenceDTO)
        {
            try
            {
                var userPreferences = await _readUserPreferencesRepository.GetPreferencesByUserIdAsync(userId);
                if (userPreferences == null)
                {
                    return Result<UserPreferenceDTO>.FailureResult("User preferences not found.");
                }
                if (updateUserPreferenceDTO.WakeUpTime.HasValue)
                    userPreferences.WakeUpTime = updateUserPreferenceDTO.WakeUpTime.Value;
                if (updateUserPreferenceDTO.SleepTime.HasValue)
                    userPreferences.SleepTime = updateUserPreferenceDTO.SleepTime.Value;
                if (updateUserPreferenceDTO.ProcrestinateFrequency.HasValue)
                    userPreferences.ProcrestinateFrequency = updateUserPreferenceDTO.ProcrestinateFrequency.Value;
                if (updateUserPreferenceDTO.FocusDifficulty.HasValue)
                    userPreferences.FocusDifficulty = updateUserPreferenceDTO.FocusDifficulty.Value;
                if (updateUserPreferenceDTO.MotivationalFactors.HasValue)
                    userPreferences.MotivationalFactors = updateUserPreferenceDTO.MotivationalFactors.Value;

                await _writeUserPreferencesRepository.UpdateAsync(userPreferences);

                var userPreferenceDTO = new UserPreferenceDTO
                {
                    WakeUpTime = userPreferences.WakeUpTime,
                    SleepTime = userPreferences.SleepTime,
                    ProcrestinateFrequency = userPreferences.ProcrestinateFrequency,
                    FocusDifficulty = userPreferences.FocusDifficulty,
                    MotivationalFactors = userPreferences.MotivationalFactors
                };
                return Result<UserPreferenceDTO>.SuccessResult(userPreferenceDTO, "User preferences updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating user preferences for userId: {UserId}", userId);
                return Result<UserPreferenceDTO>.FailureResult("An error occurred while updating user preferences.");
            }
        }
    }
}
