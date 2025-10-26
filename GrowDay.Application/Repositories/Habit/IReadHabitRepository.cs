using GrowDay.Application.Repositories.Common;
using GrowDay.Domain.Entities.Concretes;

namespace GrowDay.Application.Repositories
{
    public interface IReadHabitRepository:IReadGenericRepository<Habit>
    {
        Task<Habit?> GetByTitleAsync(string title);
        Task<IEnumerable<Habit>>GetAllHabitsListAsync(int pageIndex=0,int pageSize=10);
        Task<Habit?> GetByTitleAndUserAsync(string title,string userId);
    }
}
