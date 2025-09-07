using GrowDay.Application.Repositories.Common;
using GrowDay.Domain.Entities.Concretes;

namespace GrowDay.Application.Repositories
{
    public interface IReadTaskRepository : IReadGenericRepository<TaskEntity>
    {
        Task<ICollection<TaskEntity>>GetByHabitIdAsync(string habitId);
    }
}
