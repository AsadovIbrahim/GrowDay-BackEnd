using GrowDay.Application.Services;
using GrowDay.Domain.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GrowDay.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="User")]
    public class UserHabitController : ControllerBase
    {
        private readonly IUserHabitService _userHabitService;
        public UserHabitController(IUserHabitService userHabitService)
        {
            _userHabitService = userHabitService;
        }
        [HttpGet("GetMyHabits")]

        public async Task<IActionResult> GetAllUserHabits()
        {
            var userHabits = await _userHabitService.GetAllUserHabitAsync();
            return Ok(userHabits);
        }
        [HttpGet("{userHabitId}")]
        public async Task<IActionResult> GetUserHabitById(string userHabitId)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is required.");
            }
            var result = await _userHabitService.GetUserHabitByIdAsync(userId, userHabitId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpPost("CreateSharedHabit")]
        public async Task<IActionResult> AddUserHabit([FromBody]AddUserHabitDTO addUserHabitDTO)
        {
            var userId = GetUserId();
            
            var result = await _userHabitService.AddUserHabitAsync(userId, addUserHabitDTO);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpPost("CreateMyOwnHabit")]
        public async Task<IActionResult> AddUserOwnHabit([FromBody]AddUserOwnHabitDTO addUserOwnHabitDTO)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is required.");
            }
            var result = await _userHabitService.AddUserOwnHabitAsync(userId, addUserOwnHabitDTO);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpDelete("{userHabitId}")]
        public async Task<IActionResult> RemoveUserHabit(string userHabitId)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is required.");
            }
            var result = await _userHabitService.RemoveUserHabitAsync(userId, userHabitId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpGet("exists/{userHabitId}")]
        public async Task<IActionResult> IsUserHabitExists(string userHabitId)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is required.");
            }
            var result = await _userHabitService.IsUserHabitExistsAsync(userId, userHabitId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpPost("Complete/{userHabitId}")]
        public async Task<IActionResult> CompleteUserHabit(string userHabitId)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is required.");
            }
            var result = await _userHabitService.CompleteHabitAsync(userId, userHabitId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpDelete("ClearMyHabits")]
        public async Task<IActionResult> ClearUserHabits()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is required.");
            }
            var result = await _userHabitService.ClearUserHabitsAsync(userId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpPut("{userHabitId}")]
        public async Task<IActionResult> UpdateUserHabit(string userHabitId, [FromBody] UpdateUserHabitDTO updateUserHabitDTO)
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is required.");
            }

            var result = await _userHabitService.UpdateUserHabitAsync(userId, userHabitId, updateUserHabitDTO);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        protected string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    }
}
