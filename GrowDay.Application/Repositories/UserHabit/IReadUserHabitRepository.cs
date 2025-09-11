using GrowDay.Application.Repositories.Common;
using GrowDay.Domain.Entities.Concretes;

namespace GrowDay.Application.Repositories
{
    public interface IReadUserHabitRepository:IReadGenericRepository<UserHabit>
    {
        Task<UserHabit?> GetByUserAndHabitAsync(string userId, string habitId);
        Task<IEnumerable<UserHabit>> GetByUserIdAsync(string userId);
        Task<IEnumerable<UserHabit>> GetByHabitIdAsync(string habitId);
        Task<ICollection<UserHabit>> GetAllActiveUserHabitsAsync();
    }
}
