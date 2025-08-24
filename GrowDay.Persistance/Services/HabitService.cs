using GrowDay.Application.Repositories;
using GrowDay.Application.Services;
using GrowDay.Domain.DTO;
using GrowDay.Domain.Entities.Concretes;
using GrowDay.Domain.Helpers;
using Microsoft.Extensions.Logging;

namespace GrowDay.Persistance.Services
{
    public class HabitService : IHabitService
    {
        protected readonly IWriteHabitRepository _habitRepository;
        protected readonly IReadHabitRepository _readHabitRepository;
        protected readonly ILogger<HabitService> _logger;

        public HabitService(IWriteHabitRepository habitRepository, ILogger<HabitService> logger, IReadHabitRepository readHabitRepository)
        {
            _habitRepository = habitRepository;
            _logger = logger;
            _readHabitRepository = readHabitRepository;
        }

        public async Task<Result> ClearAllHabitsAsync()
        {
            try
            {
                var habits = await _readHabitRepository.GetAllAsync();
                if (habits == null || !habits.Any())
                {
                    return Result.FailureResult("No habits found to clear.");
                }
                foreach (var habit in habits)
                {
                    foreach (var userHabit in habit.UserHabits!)
                    {
                        userHabit.IsDeleted = true;
                    }
                    await _habitRepository.DeleteAsync(habit);
                }
                return Result.SuccessResult("All habits cleared successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing habit");
                return Result.FailureResult("An error occurred while clearing the habit.");
            }
        }
        public async Task<Result<HabitDTO>> CreateHabitAsync(CreateHabitDTO dto)
        {
            try
            {
                var existingHabit = await _readHabitRepository.GetByTitleAsync(dto.Title);
                if (existingHabit != null)
                {
                    return Result<HabitDTO>.FailureResult("A habit with this title already exists.");
                }
                var habit = new Habit
                {
                    Title = dto.Title,
                    Description = dto.Description,
                    Frequency = dto.Frequency,
                    StartDate = dto.StartDate,
                    EndDate = dto.EndDate,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                };
                await _habitRepository.AddAsync(habit);
                var result = new HabitDTO
                {
                    Id = habit.Id,
                    Title = habit.Title,
                    Description = habit.Description,
                    Frequency = habit.Frequency,
                    IsActive = habit.IsActive,
                    StartDate = habit.StartDate,
                    EndDate = habit.EndDate,
                };
                return Result<HabitDTO>.SuccessResult(result, "Habit created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating habit");
                return Result<HabitDTO>.FailureResult("An error occurred while creating the habit.");
            }

        }
        public async Task<Result> DeleteHabitAsync(string habitId)
        {
            try
            {
                var habit = await _readHabitRepository.GetByIdAsync(habitId);
                if (habit == null)
                {
                    return Result.FailureResult("Habit not found.");
                }
                await _habitRepository.DeleteAsync(habit);
                return Result.SuccessResult("Habit deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting habit");
                return Result.FailureResult("An error occurred while deleting the habit.");
            }
        }
        public async Task<Result<IEnumerable<HabitDTO>>> GetAllHabitsAsync()
        {
            try
            {
                var habits = await _readHabitRepository.GetAllAsync();
                if (habits == null || !habits.Any())
                {
                    return Result<IEnumerable<HabitDTO>>.FailureResult("No habits found.");
                }
                var habitDtos = habits.Select(h => new HabitDTO
                {
                    Id = h.Id,
                    Title = h.Title,
                    Description = h.Description,
                    Frequency = h.Frequency,
                    IsActive = h.IsActive,
                    StartDate = h.StartDate,
                    EndDate = h.EndDate
                }).ToList();
                return Result<IEnumerable<HabitDTO>>.SuccessResult(habitDtos, "Habits retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving habits");
                return Result<IEnumerable<HabitDTO>>.FailureResult("An error occurred while retrieving habits.");
            }
        }
        public async Task<Result<HabitDTO>> GetHabitByIdAsync(string habitId)
        {
            try
            {
                var habit = await _readHabitRepository.GetByIdAsync(habitId);
                if (habit == null)
                {
                    return Result<HabitDTO>.FailureResult("Habit not found.");
                }
                return Result<HabitDTO>.SuccessResult(new HabitDTO
                {
                    Id = habit.Id,
                    Title = habit.Title,
                    Description = habit.Description,
                    Frequency = habit.Frequency,
                    IsActive = habit.IsActive,
                    StartDate = habit.StartDate,
                    EndDate = habit.EndDate
                }, "Habit retrieved successfully.");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving habit by ID");
                return Result<HabitDTO>.FailureResult("An error occurred while retrieving the habit.");
            }
        }

        public async Task<Result<HabitDTO>> UpdateHabitAsync(UpdateHabitDTO dto)
        {
            try
            {
                var habit = await _readHabitRepository.GetByIdAsync(dto.Id);
                if (habit == null)
                {
                    return Result<HabitDTO>.FailureResult("Habit not found.");
                }
                habit.Title = dto.Title;
                habit.Description = dto.Description;
                habit.Frequency = dto.Frequency;
                habit.IsActive = dto.IsActive;
                habit.StartDate = dto.StartDate;
                habit.EndDate = dto.EndDate;
                await _habitRepository.UpdateAsync(habit);

                return Result<HabitDTO>.SuccessResult(new HabitDTO
                {
                    Id = habit.Id,
                    Title = habit.Title,
                    Description = habit.Description,
                    Frequency = habit.Frequency,
                    IsActive = habit.IsActive,
                    StartDate = habit.StartDate,
                    EndDate = habit.EndDate
                }, "Habit updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating habit");
                return Result<HabitDTO>.FailureResult("An error occurred while updating the habit.");
            }

        }
    }
}
