using GrowDay.Domain.DTO;
using GrowDay.Domain.Helpers;

namespace GrowDay.Application.Services
{
    public interface ISuggestedHabitService
    {
        Task<Result<IEnumerable<SuggestedHabitDTO>>>GetAllSuggestedHabitsAsync();
        Task<Result<SuggestedHabitDTO>> GetSuggestedHabitByIdAsync(string id);
        Task<Result<SuggestedHabitDTO>> CreateSuggestedHabitAsync(CreateSuggestedHabitDTO createSuggestedHabitDTO);
        Task<Result<SuggestedHabitDTO>> UpdateSuggestedHabitAsync(UpdateSuggestedHabitDTO updateSuggestedHabitDTO);
        Task<Result> DeleteSuggestedHabitAsync(string id);
        Task<Result<IEnumerable<SuggestedHabitDTO>>> GetSuggestedHabitsForUserAsync(string userId);

    }
}
