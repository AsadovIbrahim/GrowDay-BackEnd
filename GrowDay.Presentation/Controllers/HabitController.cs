using GrowDay.Application.Services;
using GrowDay.Domain.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrowDay.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HabitController : ControllerBase
    {
        private readonly IHabitService _habitService;

        public HabitController(IHabitService habitService)
        {
            _habitService = habitService;
        }
        [HttpGet("GetAllHabits")]
        public async Task<IActionResult> GetAllHabits()
        {
            var habits = await _habitService.GetAllHabitsAsync();
            return Ok(habits);
        }
        [HttpGet("{habitId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetHabitById(string habitId)
        {
            var habit = await _habitService.GetHabitByIdAsync(habitId);
            if (habit == null)
            {
                return NotFound();
            }
            return Ok(habit);
        }
        [HttpPost("CreateHabit")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateHabit([FromBody] CreateHabitDTO dto)
        {
            var result = await _habitService.CreateHabitAsync(dto);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpDelete("{habitId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteHabit(string habitId)
        {
            if (string.IsNullOrEmpty(habitId))
            {
                return BadRequest("Habit ID is required.");
            }
            var result = await _habitService.DeleteHabitAsync(habitId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpDelete("ClearAllHabits")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ClearHabits()
        {
            var result = await _habitService.ClearAllHabitsAsync();
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("UpdateHabit")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateHabit([FromBody] UpdateHabitDTO dto)
        {
            if (dto == null)
            {
                return BadRequest("Habit data is required.");
            }
            var result = await _habitService.UpdateHabitAsync(dto);
            if (result == null)
            {
                return NotFound("Habit not found.");
            }
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

    }
}
