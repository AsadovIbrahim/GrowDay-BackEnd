using GrowDay.Application.Repositories.Common;
using GrowDay.Domain.Entities.Concretes;

namespace GrowDay.Application.Repositories
{
    public interface IReadTaskRepository : IReadGenericRepository<TaskEntity>
    {
        Task<ICollection<TaskEntity>>GetByHabitIdAsync(string habitId);
        Task<IEnumerable<TaskEntity>>GetAllTasksAsync(int pageIndex=0,int pageSize=10);
    }
}
