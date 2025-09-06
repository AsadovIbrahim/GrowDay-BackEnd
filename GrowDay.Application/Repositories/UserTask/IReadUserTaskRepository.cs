using GrowDay.Domain.Entities.Concretes;
using GrowDay.Application.Repositories.Common;

namespace GrowDay.Application.Repositories
{
    public interface IReadUserTaskRepository : IReadGenericRepository<UserTask>
    {
        Task<UserTask?> GetUserTaskByIdAsync(string userId, string userTaskId);
        Task<UserTask?> GetUserTaskByTaskIdAndUserIdAsync(string userId, string taskId);
        Task<int> GetCompletedUserTaskCountByUserIdAsync(string userId);
        Task<IEnumerable<UserTask>> GetUserTasksByUserIdAsync(string userId);

    }
}
