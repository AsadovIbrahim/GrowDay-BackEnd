using GrowDay.Application.Services;
using GrowDay.Domain.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrowDay.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        protected readonly ITaskService _taskService;
        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }
        [HttpPost("CreateTask")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskDTO createTaskDTO)
        {
            var result = await _taskService.CreateTaskAsync(createTaskDTO);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpDelete("DeleteTask/{taskId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTask(string taskId)
        {
            var result = await _taskService.DeleteTaskAsync(taskId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpGet("GetAllTasks")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllTasks()
        {
            var result = await _taskService.GetAllTasksAsync();
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpGet("GetTaskById/{taskId}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetTaskById(string taskId)
        {
            var result = await _taskService.GetTaskByIdAsync(taskId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpPut("UpdateTask")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateTask([FromBody] UpdateTaskDTO updateTaskDTO)
        {
            var result = await _taskService.UpdateTaskAsync(updateTaskDTO);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpDelete("ClearAllTasks")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ClearAllTasks()
        {
            var result = await _taskService.ClearAllTasksAsync();
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
    }
}
