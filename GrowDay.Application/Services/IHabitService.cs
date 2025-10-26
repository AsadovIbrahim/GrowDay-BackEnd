using GrowDay.Domain.DTO;
using GrowDay.Domain.Helpers;

namespace GrowDay.Application.Services
{
    public interface IHabitService
    {
        Task<Result<HabitDTO>> CreateHabitAsync(CreateHabitDTO dto);
        Task<Result<HabitDTO>> UpdateHabitAsync(UpdateHabitDTO dto);
        Task<Result> DeleteHabitAsync(string habitId);
        Task<Result<HabitDTO>> GetHabitByIdAsync(string habitId);
        Task<Result<IEnumerable<HabitDTO>>> GetAllHabitsAsync(int pageIndex = 0, int pageSize = 10);
        Task<Result> ClearAllHabitsAsync();

    }
}
