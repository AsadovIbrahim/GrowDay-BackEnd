using GrowDay.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrowDay.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        protected readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userService.GetAllUsersAsync();
            if (result == null || !result.Any())
            {
                return NotFound(new { Message = "No users found." });
            }
            return Ok(result);
        }
        [HttpGet("GetUserById/{userId}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            var result = await _userService.GetUserByIdAsync(userId);
            if (!result.Success)
            {
                return NotFound(new { Message = result.Message });
            }
            return Ok(result);
        }
        [HttpDelete("RemoveUser/{userId}")]
        public async Task<IActionResult> RemoveUser(string userId)
        {
            var result = await _userService.RemoveUserAsync(userId);
            if (!result.Success)
            {
                return BadRequest(new { Message = result.Message });
            }
            return Ok(new { Message = "User removed successfully." });
        }
    }
}
