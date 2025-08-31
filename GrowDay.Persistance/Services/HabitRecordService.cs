using GrowDay.Application.Repositories;
using GrowDay.Application.Services;
using GrowDay.Domain.DTO;
using GrowDay.Domain.Entities.Concretes;
using GrowDay.Domain.Helpers;
using Microsoft.Extensions.Logging;

namespace GrowDay.Persistance.Services
{
    public class HabitRecordService : IHabitRecordService
    {
        protected readonly IReadHabitRecordRepository _readHabitRecordRepository;
        protected readonly IWriteHabitRecordRepository _writeHabitRecordRepository;
        protected readonly ILogger<HabitRecordService> _logger;

        public HabitRecordService(IReadHabitRecordRepository readHabitRecordRepository, IWriteHabitRecordRepository writeHabitRecordRepository,
            ILogger<HabitRecordService> logger)
        {
            _logger = logger;
            _readHabitRecordRepository = readHabitRecordRepository;
            _writeHabitRecordRepository = writeHabitRecordRepository;
        }

        public async Task<Result> ClearAllHabitRecordsAsync(string userId)
        {
            try
            {
                var habitRecords = await _readHabitRecordRepository.GetAllByUserAsync(userId);
                if (habitRecords == null || !habitRecords.Any())
                {
                    return Result.FailureResult("No habit records found for the user.");
                }
                foreach (var record in habitRecords)
                {
                    await _writeHabitRecordRepository.DeleteAsync(record);
                }
                return Result.SuccessResult("All habit records cleared successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while clearing all habit records");
                return Result.FailureResult("An error occurred while clearing habit records.");
            }
        }

        public async Task<Result> CreateHabitRecordAsync(AddHabitRecordDTO addHabitRecordDTO)
        {
            try
            {
                var existingRecord = await _readHabitRecordRepository.GetAllByHabitIdAsync(addHabitRecordDTO.UserHabitId);
                if (existingRecord.Any(r => r.Date.Date == addHabitRecordDTO.Date.Date))
                {
                    return Result.FailureResult("A habit record for this date already exists.");
                }
                var habitRecord = new HabitRecord
                {
                    UserHabitId = addHabitRecordDTO.UserHabitId,
                    Date = addHabitRecordDTO.Date,
                    IsCompleted = addHabitRecordDTO.IsCompleted,
                    Note = addHabitRecordDTO.Note
                };
                await _writeHabitRecordRepository.AddAsync(habitRecord);
                return Result.SuccessResult("Habit record added successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding habit record");
                return Result.FailureResult("An error occurred while adding the habit record.");
            }
        }

        public async Task<Result<HabitRecordDTO>> DeleteHabitRecordAsync(string habitRecordId)
        {
            try
            {
                var habitRecord = await _readHabitRecordRepository.GetByIdAsync(habitRecordId);
                if (habitRecord == null)
                {
                    return Result<HabitRecordDTO>.FailureResult("Habit record not found.");
                }
                habitRecord.IsDeleted = true;
                await _writeHabitRecordRepository.UpdateAsync(habitRecord);
                var habitRecordDTO = new HabitRecordDTO
                {
                    Id = habitRecord.Id,
                    UserHabitId = habitRecord.UserHabitId,
                    Date = habitRecord.Date,
                    IsCompleted = habitRecord.IsCompleted,
                    Note = habitRecord.Note
                };
                return Result<HabitRecordDTO>.SuccessResult(habitRecordDTO, "Habit record deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting habit record");
                return Result<HabitRecordDTO>.FailureResult("An error occurred while deleting the habit record.");
            }
        }

        

        public async Task<Result<HabitRecordDTO>> GetHabitRecordByIdAsync(string habitRecordId)
        {
            try
            {
                var habitRecord = await _readHabitRecordRepository.GetByIdAsync(habitRecordId);
                if (habitRecord == null)
                {
                    return Result<HabitRecordDTO>.FailureResult("Habit record not found.");
                }

                var habitRecordDTO = new HabitRecordDTO
                {
                    Id = habitRecord.Id,
                    UserHabitId = habitRecord.UserHabitId,
                    Date = habitRecord.Date,
                    IsCompleted = habitRecord.IsCompleted,
                    Note = habitRecord.Note
                };

                return Result<HabitRecordDTO>.SuccessResult(habitRecordDTO, "Habit record retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving habit record by ID");
                return Result<HabitRecordDTO>.FailureResult("An error occurred while retrieving the habit record.");
            }
        }

        public async Task<Result<List<HabitRecordDTO>>> GetHabitRecordByUserAsync(string userId)
        {
            try
            {
                var habitRecords = await _readHabitRecordRepository.GetAllByUserAsync(userId);
                if (habitRecords == null || !habitRecords.Any())
                {
                    return Result<List<HabitRecordDTO>>.FailureResult("No habit records found for the user.");
                }
                var habitRecordDTOs = habitRecords.Select(hr => new HabitRecordDTO
                {
                    Id = hr.Id,
                    UserHabitId = hr.UserHabitId,
                    Date = hr.Date,
                    IsCompleted = hr.IsCompleted,
                    Note = hr.Note
                }).ToList();
                return Result<List<HabitRecordDTO>>.SuccessResult(habitRecordDTOs, "Habit records retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving habit records by user");
                return Result<List<HabitRecordDTO>>.FailureResult("An error occurred while retrieving the habit records.");
            }
        }

        public async Task<Result<HabitRecordDTO>> UpdateHabitRecordAsync(string habitRecordId,UpdateHabitRecordDTO updateHabitRecordDTO)
        {
            try
            {
                var habitRecord = await _readHabitRecordRepository.GetByIdAsync(habitRecordId);
                if (habitRecord == null)
                {
                    return Result<HabitRecordDTO>.FailureResult("Habit record not found.");
                }
                habitRecord.UserHabitId = updateHabitRecordDTO.UserHabitId;
                habitRecord.Date = updateHabitRecordDTO.Date;
                habitRecord.IsCompleted = updateHabitRecordDTO.IsCompleted;
                habitRecord.Note = updateHabitRecordDTO.Note;

                await _writeHabitRecordRepository.UpdateAsync(habitRecord);
                var habitRecordDTO = new HabitRecordDTO
                {
                    Id = habitRecord.Id,
                    UserHabitId = habitRecord.UserHabitId,
                    Date = habitRecord.Date,
                    IsCompleted = habitRecord.IsCompleted,
                    Note = habitRecord.Note
                };
                return Result<HabitRecordDTO>.SuccessResult(habitRecordDTO, "Habit record updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating habit record");
                return Result<HabitRecordDTO>.FailureResult("An error occurred while updating the habit record.");
            }
        }
    }
}
