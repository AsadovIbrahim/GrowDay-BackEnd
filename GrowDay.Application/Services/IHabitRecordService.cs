using GrowDay.Domain.DTO;
using GrowDay.Domain.Helpers;

namespace GrowDay.Application.Services
{
    public interface IHabitRecordService
    {
        Task<Result> CreateHabitRecordAsync(AddHabitRecordDTO addHabitRecordDTO);
        Task<Result<HabitRecordDTO>> UpdateHabitRecordAsync(string habitRecordId,UpdateHabitRecordDTO updateHabitRecordDTO);
        Task<Result<HabitRecordDTO>> GetHabitRecordByIdAsync(string habitRecordId);
        Task<Result<HabitRecordDTO>> DeleteHabitRecordAsync(string habitRecordId);
        Task<Result<List<HabitRecordDTO>>> GetHabitRecordByUserAsync(string userId);
    }
}
