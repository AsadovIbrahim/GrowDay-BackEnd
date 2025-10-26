using GrowDay.Domain.DTO;
using GrowDay.Domain.Helpers;

namespace GrowDay.Application.Services
{
    public interface ITaskService
    {
        Task<Result<IEnumerable<TaskDTO>>> GetAllTasksAsync(int pageIndex = 0, int pageSize = 10);
        Task<Result<TaskDTO>> GetTaskByIdAsync(string taskId);
        Task<Result<TaskDTO>> CreateTaskAsync(CreateTaskDTO createTaskDTO);
        Task<Result<TaskDTO>> UpdateTaskAsync(UpdateTaskDTO updateTaskDTO);
        Task<Result> DeleteTaskAsync(string taskId);
        Task<Result> ClearAllTasksAsync();
    }
}
