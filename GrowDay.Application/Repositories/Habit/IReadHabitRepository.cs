using GrowDay.Application.Repositories.Common;
using GrowDay.Domain.Entities.Concretes;

namespace GrowDay.Application.Repositories
{
    public interface IReadHabitRepository:IReadGenericRepository<Habit>
    {
        Task<Habit?> GetByTitleAsync(string title);
        Task<Habit?> GetByTitleAndUserAsync(string title,string userId);
    }
}
