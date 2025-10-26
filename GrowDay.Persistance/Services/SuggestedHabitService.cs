using GrowDay.Application.Repositories;
using GrowDay.Application.Services;
using GrowDay.Domain.DTO;
using GrowDay.Domain.Entities.Concretes;
using GrowDay.Domain.Enums;
using GrowDay.Domain.Helpers;
using Microsoft.Extensions.Logging;

namespace GrowDay.Persistance.Services
{
    public class SuggestedHabitService : ISuggestedHabitService
    {
        protected readonly IWriteSuggestedHabitRepository _writeSuggestedHabitRepository;
        protected readonly IReadSuggestedHabitRepository _readSuggestedHabitRepository;
        protected readonly IReadUserHabitRepository _readUserHabitRepository;
        protected readonly IWriteUserHabitRepository _writeUserHabitRepository;
        protected readonly IReadUserPreferencesRepository _readUserPreferencesRepository;
        protected readonly IUserPreferencesService _userPreferencesService;
        protected readonly ILogger<SuggestedHabitService> _logger;
        public SuggestedHabitService(IWriteSuggestedHabitRepository writeSuggestedHabitRepository, IReadSuggestedHabitRepository readSuggestedHabitRepository,
            ILogger<SuggestedHabitService> logger, IUserPreferencesService userPreferencesService, IReadUserPreferencesRepository readUserPreferencesRepository,
            IReadUserHabitRepository readUserHabitRepository, IWriteUserHabitRepository writeUserHabitRepository)
        {
            _writeSuggestedHabitRepository = writeSuggestedHabitRepository;
            _readSuggestedHabitRepository = readSuggestedHabitRepository;
            _logger = logger;
            _userPreferencesService = userPreferencesService;
            _readUserPreferencesRepository = readUserPreferencesRepository;
            _readUserHabitRepository = readUserHabitRepository;
            _writeUserHabitRepository = writeUserHabitRepository;
        }
        public async Task<Result<SuggestedHabitDTO>> CreateSuggestedHabitAsync(CreateSuggestedHabitDTO createSuggestedHabitDTO)
        {
            try
            {
                var suggestedHabit = new SuggestedHabit
                {
                    Title = createSuggestedHabitDTO.Title,
                    Description = createSuggestedHabitDTO.Description,
                    Frequency = createSuggestedHabitDTO.Frequency,
                    NotificationTime = createSuggestedHabitDTO.NotificationTime,
                    DurationInMinutes = createSuggestedHabitDTO.DurationInMinutes,
                    StartDate = createSuggestedHabitDTO.StartDate,
                    EndDate = createSuggestedHabitDTO.EndDate,
                    TargetValue = createSuggestedHabitDTO.TargetValue,
                    IncrementValue = createSuggestedHabitDTO.IncrementValue,
                    Unit = createSuggestedHabitDTO.Unit,
                    IsActive = true,
                    Criteria =createSuggestedHabitDTO.Criteria??HabitCriteria.None,
                    IsDeleted = false
                };
                await _writeSuggestedHabitRepository.AddAsync(suggestedHabit);

                var suggestedHabitDTO = new SuggestedHabitDTO
                {
                    Id = suggestedHabit.Id,
                    Title = suggestedHabit.Title,
                    Description = suggestedHabit.Description,
                    Frequency = suggestedHabit.Frequency,
                    NotificationTime = suggestedHabit.NotificationTime,
                    DurationInMinutes = suggestedHabit.DurationInMinutes,
                    StartDate = suggestedHabit.StartDate,
                    EndDate = suggestedHabit.EndDate,
                    TargetValue = suggestedHabit.TargetValue,
                    IncrementValue = suggestedHabit.IncrementValue,
                    Unit = suggestedHabit.Unit,
                    IsActive = suggestedHabit.IsActive,
                    Criteria = suggestedHabit.Criteria
                };
                return Result<SuggestedHabitDTO>.SuccessResult(suggestedHabitDTO, "Suggested habit created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating suggested habit");
                return Result<SuggestedHabitDTO>.FailureResult("An error occurred while creating the suggested habit.");
            }
        }

        

        public async Task<Result> DeleteSuggestedHabitAsync(string id)
        {
            try
            {
                var suggestedHabit = await _readSuggestedHabitRepository.GetByIdAsync(id);
                if (suggestedHabit == null || suggestedHabit.IsDeleted)
                {
                    return Result.FailureResult("Suggested habit not found.");
                }
                suggestedHabit.IsDeleted = true;
                await _writeSuggestedHabitRepository.DeleteAsync(suggestedHabit);
                return Result.SuccessResult("Suggested habit deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting suggested habit");
                return Result.FailureResult("An error occurred while deleting the suggested habit.");
            }
        }

        public async Task<Result<IEnumerable<SuggestedHabitDTO>>> GetAllSuggestedHabitsAsync(int pageIndex=0,int pageSize=10)
        {
            try
            {
                var suggestedHabits = await _readSuggestedHabitRepository.GetSuggestedHabitsAsync(pageIndex,pageSize);
                var suggestedHabitDTOs = suggestedHabits!
                    .Where(sh => !sh.IsDeleted)
                    .Select(sh => new SuggestedHabitDTO
                    {
                        Id = sh.Id,
                        Title = sh.Title,
                        Description = sh.Description,
                        Frequency = sh.Frequency,
                        NotificationTime = sh.NotificationTime,
                        DurationInMinutes = sh.DurationInMinutes,
                        StartDate = sh.StartDate,
                        EndDate = sh.EndDate,
                        TargetValue = sh.TargetValue,
                        IncrementValue = sh.IncrementValue,
                        Unit = sh.Unit,
                        IsActive = sh.IsActive,
                        Criteria = sh.Criteria
                    }).ToList();

                return Result<IEnumerable<SuggestedHabitDTO>>.SuccessResult(suggestedHabitDTOs, "Suggested habits retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving suggested habits");
                return Result<IEnumerable<SuggestedHabitDTO>>.FailureResult("An error occurred while retrieving suggested habits.");
            }
        }

        public async Task<Result<SuggestedHabitDTO>> GetSuggestedHabitByIdAsync(string id)
        {
            try
            {
                var suggestedHabit = await _readSuggestedHabitRepository.GetByIdAsync(id);
                if (suggestedHabit == null || suggestedHabit.IsDeleted)
                {
                    return Result<SuggestedHabitDTO>.FailureResult("Suggested habit not found.");
                }
                var suggestedHabitDTO = new SuggestedHabitDTO
                {
                    Id = suggestedHabit.Id,
                    Title = suggestedHabit.Title,
                    Description = suggestedHabit.Description,
                    Frequency = suggestedHabit.Frequency,
                    NotificationTime = suggestedHabit.NotificationTime,
                    DurationInMinutes = suggestedHabit.DurationInMinutes,
                    StartDate = suggestedHabit.StartDate,
                    EndDate = suggestedHabit.EndDate,
                    TargetValue = suggestedHabit.TargetValue,
                    IncrementValue = suggestedHabit.IncrementValue,
                    Unit = suggestedHabit.Unit,
                    IsActive = suggestedHabit.IsActive,
                    Criteria = suggestedHabit.Criteria
                };
                return Result<SuggestedHabitDTO>.SuccessResult(suggestedHabitDTO, "Suggested habit retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving suggested habit by id");
                return Result<SuggestedHabitDTO>.FailureResult("An error occurred while retrieving the suggested habit.");
            }
        }

        public async Task<Result<IEnumerable<SuggestedHabitDTO>>> GetSuggestedHabitsForUserAsync(string userId, int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                var preferences = await _readUserPreferencesRepository.GetPreferencesByUserIdAsync(userId);
                if (preferences == null)
                    return Result<IEnumerable<SuggestedHabitDTO>>.FailureResult("User preferences not found.");

                var allSuggestedHabits = await _readSuggestedHabitRepository.GetSuggestedUserHabitsAsync(pageIndex,pageSize);

                var matchedHabits = allSuggestedHabits!.Where(habit =>
                    MatchesCriteria(habit, preferences));

                var mapped = matchedHabits.Select(h => new SuggestedHabitDTO
                {
                    Id = h.Id,
                    Title = h.Title,
                    Description = h.Description,
                    Frequency = h.Frequency,
                    Criteria = h.Criteria,
                    TargetValue = h.TargetValue,
                    IncrementValue = h.IncrementValue,
                    Unit = h.Unit,
                });

                return Result<IEnumerable<SuggestedHabitDTO>>.SuccessResult(mapped, "Matched habits retrieved.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching suggested habits for user {UserId}", userId);
                return Result<IEnumerable<SuggestedHabitDTO>>.FailureResult("An error occurred while fetching suggested habits.");
            }
        }


        public async Task<Result<SuggestedHabitDTO>> UpdateSuggestedHabitAsync(UpdateSuggestedHabitDTO updateSuggestedHabitDTO)
        {
            try
            {
                var suggestedHabit = await _readSuggestedHabitRepository.GetByIdAsync(updateSuggestedHabitDTO.Id);
                if (suggestedHabit == null || suggestedHabit.IsDeleted)
                {
                    return Result<SuggestedHabitDTO>.FailureResult("Suggested habit not found.");
                }
                suggestedHabit.Title = updateSuggestedHabitDTO.Title ?? suggestedHabit.Title;
                suggestedHabit.Description = updateSuggestedHabitDTO.Description ?? suggestedHabit.Description;
                suggestedHabit.Frequency = updateSuggestedHabitDTO.Frequency ?? suggestedHabit.Frequency;
                suggestedHabit.NotificationTime = updateSuggestedHabitDTO.NotificationTime ?? suggestedHabit.NotificationTime;
                suggestedHabit.DurationInMinutes = updateSuggestedHabitDTO.DurationInMinutes ?? suggestedHabit.DurationInMinutes;
                suggestedHabit.StartDate = updateSuggestedHabitDTO.StartDate ?? suggestedHabit.StartDate;
                suggestedHabit.EndDate = updateSuggestedHabitDTO.EndDate ?? suggestedHabit.EndDate;
                suggestedHabit.Criteria = updateSuggestedHabitDTO.Criteria ?? suggestedHabit.Criteria;
                suggestedHabit.TargetValue = updateSuggestedHabitDTO.TargetValue ?? suggestedHabit.TargetValue;
                suggestedHabit.IncrementValue = updateSuggestedHabitDTO.IncrementValue ?? suggestedHabit.IncrementValue;
                suggestedHabit.Unit = updateSuggestedHabitDTO.Unit ?? suggestedHabit.Unit;

                await _writeSuggestedHabitRepository.UpdateAsync(suggestedHabit);

                var suggestedHabitDTO = new SuggestedHabitDTO
                {
                    Id = suggestedHabit.Id,
                    Title = suggestedHabit.Title,
                    Description = suggestedHabit.Description,
                    Frequency = suggestedHabit.Frequency,
                    NotificationTime = suggestedHabit.NotificationTime,
                    DurationInMinutes = suggestedHabit.DurationInMinutes,
                    StartDate = suggestedHabit.StartDate,
                    EndDate = suggestedHabit.EndDate,
                    IsActive = suggestedHabit.IsActive,
                    Criteria = suggestedHabit.Criteria,
                    TargetValue = suggestedHabit.TargetValue,
                    IncrementValue = suggestedHabit.IncrementValue,
                    Unit = suggestedHabit.Unit
                };
                return Result<SuggestedHabitDTO>.SuccessResult(suggestedHabitDTO, "Suggested habit updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating suggested habit");
                return Result<SuggestedHabitDTO>.FailureResult("An error occurred while updating the suggested habit.");
            }
        }

        private bool MatchesCriteria(SuggestedHabit habit, UserPreferences preferences)
        {
            switch (habit.Criteria)
            {
                case HabitCriteria.MorningPerson:
                    return preferences.WakeUpTime < TimeSpan.FromHours(7);

                case HabitCriteria.NightOwl:
                    return preferences.SleepTime > TimeSpan.FromHours(23);

                case HabitCriteria.HighProcrastination:
                    return preferences.ProcrestinateFrequency == ProcrastinateFrequency.Always
                           || preferences.ProcrestinateFrequency == ProcrastinateFrequency.Sometimes;

                case HabitCriteria.LowProcrastination:
                    return preferences.ProcrestinateFrequency == ProcrastinateFrequency.Rarely
                           || preferences.ProcrestinateFrequency == ProcrastinateFrequency.Never;

                case HabitCriteria.FocusDifficultyHigh:
                    return preferences.FocusDifficulty == FocusDifficulty.Constantly
                           || preferences.FocusDifficulty == FocusDifficulty.Occasionally;

                case HabitCriteria.LowMotivation:
                    return preferences.MotivationalFactors.HasFlag(MotivationalFactors.LackOfMotivation);

                case HabitCriteria.DigitalDistractions:
                    return preferences.MotivationalFactors.HasFlag(MotivationalFactors.DigitalDistractions);

                case HabitCriteria.None:
                default:
                    return true;
            }
        }

    }


}
