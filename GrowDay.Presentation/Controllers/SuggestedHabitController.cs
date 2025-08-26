using GrowDay.Application.Services;
using GrowDay.Domain.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GrowDay.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuggestedHabitController : ControllerBase
    {
        protected readonly ISuggestedHabitService _suggestedHabitService;
        public SuggestedHabitController(ISuggestedHabitService suggestedHabitService)
        {
            _suggestedHabitService = suggestedHabitService;
        }

        [Authorize(Roles ="User")]
        [HttpGet("GetUserSuggestedHabits")]
        public async Task<IActionResult> GetUserSuggestedHabits()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _suggestedHabitService.GetSuggestedHabitsForUserAsync(userId!);
            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }
        [Authorize(Roles ="Admin")]
        [HttpGet("GetAllSuggestedHabits")]
        public async Task<IActionResult> GetAllSuggestedHabits()
        {
            var result = await _suggestedHabitService.GetAllSuggestedHabitsAsync();
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result);
        }
        [Authorize (Roles ="User")]
        [HttpGet("{suggestedHabitId}")]
        public async Task<IActionResult> GetSuggestedHabitById(string suggestedHabitId)
        {
            var result = await _suggestedHabitService.GetSuggestedHabitByIdAsync(suggestedHabitId);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result);
        }
        [Authorize (Roles ="Admin")]
        [HttpPost("CreateSuggestedHabit")]
        public async Task<IActionResult> CreateSuggestedHabit([FromBody] CreateSuggestedHabitDTO createSuggestedHabitDTO)
        {
            var result = await _suggestedHabitService.CreateSuggestedHabitAsync(createSuggestedHabitDTO);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result);
        }
        
        [Authorize (Roles ="Admin")]
        [HttpPut("UpdateSuggestedHabit")]
        public async Task<IActionResult> UpdateSuggestedHabit([FromBody] UpdateSuggestedHabitDTO updateSuggestedHabitDTO)
        {
            var result = await _suggestedHabitService.UpdateSuggestedHabitAsync(updateSuggestedHabitDTO);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result);
        }
        [Authorize (Roles ="Admin")]
        [HttpDelete("DeleteSuggestedHabit/{suggestedHabitId}")]
        public async Task<IActionResult> DeleteSuggestedHabit(string suggestedHabitId)
        {
            var result = await _suggestedHabitService.DeleteSuggestedHabitAsync(suggestedHabitId);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result);
        }

    }
}
