using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using GrowDay.Application.Services;
using Microsoft.AspNetCore.Authorization;

namespace GrowDay.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTaskController : ControllerBase
    {
        protected readonly IUserTaskService _userTaskService;
        public UserTaskController(IUserTaskService userTaskService)
        {
            _userTaskService = userTaskService;
        }
       
        [HttpGet("GetMyTasks")]
        [Authorize(Roles ="Admin,User")]
        public async Task<IActionResult> GetAllTasks()
        {
            var userId=User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { Message = "User is not authenticated." });
            }
            var result = await _userTaskService.GetAllTasksAsync(userId!);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpPut("CompleteTask/{userTaskId}")]
        [Authorize(Roles ="User")]
        public async Task<IActionResult> CompleteTask(string userTaskId)
        {
            var userId=User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { Message = "User is not authenticated." });
            }
            var result = await _userTaskService.CompleteTaskAsync(userId, userTaskId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
       
    }
}
