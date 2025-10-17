using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using GrowDay.Application.Services;
using Microsoft.AspNetCore.Authorization;

namespace GrowDay.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class UserActivityController : ControllerBase
    {
        protected readonly IUserActivityService _userActivityService;
        public UserActivityController(IUserActivityService userActivityService)
        {
            _userActivityService = userActivityService;
        }
        [HttpGet("GetUserActivities")]
        public async Task<IActionResult> GetUserActivities()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is required.");
            }
            var result = await _userActivityService.GetUserActivitiesAsync(userId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpDelete("{activityId}")]
        public async Task<IActionResult> DeleteUserActivity(string activityId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is required.");
            }
            if (string.IsNullOrEmpty(activityId))
            {
                return BadRequest("Activity ID is required.");
            }
            var result = await _userActivityService.DeleteActivityAsync(userId, activityId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpDelete("ClearActivities")]
        public async Task<IActionResult> ClearUserActivities()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is required.");
            }
            var result = await _userActivityService.ClearActivityAsync(userId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpGet("GetTotalPoints")]
        public async Task<IActionResult> GetUserTotalPoints()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is required.");
            }
            var result = await _userActivityService.GetUserTotalPointsAsync(userId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
    }
}
