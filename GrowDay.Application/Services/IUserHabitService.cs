using GrowDay.Domain.DTO;
using GrowDay.Domain.Helpers;

namespace GrowDay.Application.Services
{
    public interface IUserHabitService
    {
        Task<Result> AddUserHabitAsync(string userId, AddUserHabitDTO addUserHabitDTO);
        Task<Result> AddUserOwnHabitAsync(string userId, AddUserOwnHabitDTO addUserOwnHabitDTO);
        Task<Result> RemoveUserHabitAsync(string userId, string userHabitId);
        Task<Result<UserHabitDTO>> GetUserHabitByIdAsync(string userId, string userHabitId);
        Task<Result<UserHabitDTO>> UpdateUserHabitAsync(string userId, string userHabitId, UpdateUserHabitDTO updateUserHabitDTO);
        Task<Result<bool>> IsUserHabitExistsAsync(string userId, string userHabitId);
        Task<Result> ClearUserHabitsAsync(string userId);
        Task<Result<UserHabitDTO>> CompleteHabitAsync(string userId, string userHabitId, string? note = null);
        Task<Result<ICollection<HabitRecordDTO>>>GetAllCompletedHabitsAsync(string userId);
        Task<Result<List<UserHabitDTO>>> GetAllUserHabitAsync();

        Task<Result> AddFromSuggestedHabitAsync(string userId, AddSuggestedHabitDTO addSuggestedHabitDTO);

        Task<Result<bool>> IsHabitCompletedTodayAsync(string userId, string userHabitId, DateTime date);


    }
}
