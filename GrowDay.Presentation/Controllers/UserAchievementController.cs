using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using GrowDay.Application.Services;
using Microsoft.AspNetCore.Authorization;

namespace GrowDay.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="User")]
    public class UserAchievementController : ControllerBase
    {
        protected readonly IUserAchievementService _userAchievementService;
        public UserAchievementController(IUserAchievementService userAchievementService)
        {
            _userAchievementService = userAchievementService;
        }
        [HttpGet("GetMyAchievements")]
        public async Task<IActionResult> GetUserAchievements()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized("User is not authenticated.");
            var result = await _userAchievementService.GetUserAchievementsAsync(userId);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result);
        }
        [HttpDelete("DeleteMyAchievement/{userAchievementId}")]
        public async Task<IActionResult> DeleteUserAchievement(string userAchievementId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized("User is not authenticated.");
            var result = await _userAchievementService.DeleteUserAchievementAsync(userId, userAchievementId);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result);
        }
        [HttpDelete("ClearMyAchievements")]
        public async Task<IActionResult> ClearAllUserAchievements()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized("User is not authenticated.");
            var result = await _userAchievementService.ClearAllUserAchievementsAsync();
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result);
        }


    }
}
