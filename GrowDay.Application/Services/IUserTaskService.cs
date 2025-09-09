using GrowDay.Domain.DTO;
using GrowDay.Domain.Helpers;

namespace GrowDay.Application.Services
{
    public interface IUserTaskService
    {
       
        Task<Result<IEnumerable<UserTaskDTO>>>GetAllTasksAsync(string userId);
        Task<Result<UserTaskDTO>>CompleteTaskAsync(string userId,string taskId);
        Task<Result<ICollection<UserTaskDTO>>>GetCompletedTasksAsync(string userId);
        Task CheckAndGrantAchievementsAsync(string userId);
        Task<Result>DeleteUserTaskAsync(string userTaskId);

    }
}
