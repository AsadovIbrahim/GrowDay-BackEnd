using GrowDay.Application.Repositories.Common;
using GrowDay.Domain.Entities.Concretes;
using GrowDay.Domain.Enums;

namespace GrowDay.Application.Repositories
{
    public interface IReadUserHabitRepository:IReadGenericRepository<UserHabit>
    {
        Task<UserHabit?> GetByUserAndHabitAsync(string userId, string habitId);
        Task<int>GetUserHabitsCountAsync(string userId);
        Task<IEnumerable<UserHabit>> GetByUserIdAsync(string userId);
        Task<IEnumerable<UserHabit>> GetByHabitIdAsync(string habitId);
        Task<ICollection<UserHabit>> GetAllActiveUserHabitsAsync(int pageIndex=0,int pageSize=10);
    }
}
