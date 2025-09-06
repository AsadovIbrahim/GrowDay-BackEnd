using GrowDay.Domain.DTO;
using Microsoft.AspNetCore.Mvc;
using GrowDay.Application.Services;
using Microsoft.AspNetCore.Authorization;

namespace GrowDay.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AchievementController : ControllerBase
    {
        protected readonly IAchievementService _achievementService;
        public AchievementController(IAchievementService achievementService)
        {
            _achievementService = achievementService;
        }
        [HttpGet("GetAllAchievements")]
        [Authorize(Roles ="Admin,User")]
        public async Task<IActionResult> GetAllAchievements()
        {
            var result = await _achievementService.GetAllAchievementsAsync();
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpPost("CreateAchievement")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> CreateAchievement([FromBody] CreateAchievementDTO createAchievementDTO)
        {
            var result = await _achievementService.CreateAchievementAsync(createAchievementDTO);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpPut("UpdateAchievement")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> UpdateAchievement([FromBody] UpdateAchievementDTO updateAchievementDTO)
        {
            var result = await _achievementService.UpdateAchievementAsync(updateAchievementDTO);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpDelete("DeleteAchievement/{achievementId}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> DeleteAchievement(string achievementId)
        {
            var result = await _achievementService.DeleteAchievementAsync(achievementId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpDelete("ClearAllAchievements")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> ClearAllAchievements()
        {
            var result = await _achievementService.ClearAllAchievementsAsync();
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
    }
}
