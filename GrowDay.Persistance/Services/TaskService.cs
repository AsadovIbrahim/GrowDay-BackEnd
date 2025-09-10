using GrowDay.Application.Repositories;
using GrowDay.Application.Services;
using GrowDay.Domain.DTO;
using GrowDay.Domain.Entities.Concretes;
using GrowDay.Domain.Helpers;
using Microsoft.Extensions.Logging;

namespace GrowDay.Persistance.Services
{
    public class TaskService : ITaskService
    {
        protected readonly IWriteTaskRepository _writeTaskRepository;
        protected readonly IWriteUserTaskRepository _writeUserTaskRepository;
        protected readonly IReadTaskRepository _readTaskRepository;
        protected readonly IReadUserRepository _readUserRepository;
        protected readonly ILogger<TaskService> _logger;
        public TaskService(IWriteTaskRepository writeTaskRepository, IReadTaskRepository readTaskRepository, ILogger<TaskService> logger,
            IReadUserRepository readUserRepository, IWriteUserTaskRepository writeUserTaskRepository)
        {
            _writeTaskRepository = writeTaskRepository;
            _readTaskRepository = readTaskRepository;
            _logger = logger;
            _readUserRepository = readUserRepository;
            _writeUserTaskRepository = writeUserTaskRepository;
        }

        public async Task<Result> ClearAllTasksAsync()
        {
            try
            {
                var tasks = await _readTaskRepository.GetAllAsync();
                if (tasks == null || !tasks.Any())
                {
                    return Result.FailureResult("No tasks found to clear.");
                }
                foreach (var task in tasks)
                {
                    await _writeTaskRepository.DeleteAsync(task);
                }
                return Result.SuccessResult("All tasks cleared successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while clearing all tasks");
                return Result.FailureResult("An error occurred while clearing tasks.");
            }
        }

        public async Task<Result<TaskDTO>> CreateTaskAsync(CreateTaskDTO createTaskDTO)
        {
            try
            {
                var newTask = new TaskEntity
                {
                    HabitId = createTaskDTO.HabitId,
                    Title = createTaskDTO.Title,
                    Description = createTaskDTO.Description,
                    Points = createTaskDTO.Points,
                    TotalRequiredCompletions = createTaskDTO.TotalRequiredCompletions,
                    RequiredPoints = createTaskDTO.RequiredPoints,
                    StreakRequired = createTaskDTO.StreakRequired,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };
                await _writeTaskRepository.AddAsync(newTask);


                var taskDTO = new TaskDTO
                {
                    HabitId = newTask.HabitId,
                    TaskId = newTask.Id,
                    Title = newTask.Title,
                    Description = newTask.Description,
                    Points = newTask.Points,
                    TotalRequiredCompletions = newTask.TotalRequiredCompletions,
                    RequiredPoints = newTask.RequiredPoints,
                    StreakRequired = newTask.StreakRequired,
                    IsActive = newTask.IsActive
                };
                return Result<TaskDTO>.SuccessResult(taskDTO, "Task created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating task with title {Title}", createTaskDTO.Title);
                return Result<TaskDTO>.FailureResult("An error occurred while creating the task.");
            }
        }

        public async Task<Result> DeleteTaskAsync(string taskId)
        {
            try
            {
                var task = await _readTaskRepository.GetByIdAsync(taskId);
                if (task == null)
                {
                    return Result.FailureResult("Task not found.");
                }
                await _writeTaskRepository.DeleteAsync(task);
                return Result.SuccessResult("Task deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting task {TaskId}", taskId);
                return Result.FailureResult("An error occurred while deleting the task.");
            }
        }

        public async Task<Result<IEnumerable<TaskDTO>>> GetAllTasksAsync()
        {
            try
            {
                var tasks = await _readTaskRepository.GetAllAsync();
                if (!tasks.Any())
                {
                    return Result<IEnumerable<TaskDTO>>
                        .SuccessResult(Enumerable.Empty<TaskDTO>(), "No tasks found.");
                }
                var taskDTOs = tasks.Select(t => new TaskDTO
                {
                    HabitId = t.HabitId,
                    TaskId = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Points = t.Points,
                    TotalRequiredCompletions = t.TotalRequiredCompletions,
                    RequiredPoints = t.RequiredPoints,
                    StreakRequired = t.StreakRequired,
                    IsActive = t.IsActive
                }).ToList();
                return Result<IEnumerable<TaskDTO>>.SuccessResult(taskDTOs, "Tasks retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all tasks");
                return Result<IEnumerable<TaskDTO>>.FailureResult("An error occurred while retrieving tasks.");
            }

        }

        public async Task<Result<TaskDTO>> GetTaskByIdAsync(string taskId)
        {
            try
            {
                var task = await _readTaskRepository.GetByIdAsync(taskId);
                if (task == null)
                {
                    return Result<TaskDTO>.FailureResult("Task not found.");
                }
                var taskDTO = new TaskDTO
                {
                    TaskId = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    Points = task.Points,
                    TotalRequiredCompletions = task.TotalRequiredCompletions,
                    RequiredPoints = task.RequiredPoints,
                    StreakRequired = task.StreakRequired,
                    IsActive = task.IsActive
                };
                return Result<TaskDTO>.SuccessResult(taskDTO, "Task retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving task {TaskId}", taskId);
                return Result<TaskDTO>.FailureResult("An error occurred while retrieving the task.");
            }
        }

        public async Task<Result<TaskDTO>> UpdateTaskAsync(UpdateTaskDTO updateTaskDTO)
        {
            try
            {
                var task = await _readTaskRepository.GetByIdAsync(updateTaskDTO.TaskId);
                if (task == null)
                {
                    return Result<TaskDTO>.FailureResult("Task not found.");
                }
                task.HabitId = updateTaskDTO.HabitId;
                task.Title = updateTaskDTO.Title;
                task.Description = updateTaskDTO.Description;
                task.Points = updateTaskDTO.Points;
                task.IsActive = updateTaskDTO.IsActive;
                await _writeTaskRepository.UpdateAsync(task);
                var taskDTO = new TaskDTO
                {
                    HabitId = task.HabitId,
                    TaskId = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    Points = task.Points,
                    TotalRequiredCompletions = task.TotalRequiredCompletions,
                    RequiredPoints = task.RequiredPoints,
                    StreakRequired = task.StreakRequired,
                    IsActive = task.IsActive
                };
                return Result<TaskDTO>.SuccessResult(taskDTO, "Task updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating task {TaskId}", updateTaskDTO.TaskId);
                return Result<TaskDTO>.FailureResult("An error occurred while updating the task.");
            }
        }
    }
}
