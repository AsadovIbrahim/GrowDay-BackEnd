using GrowDay.Application.Services;
using GrowDay.Domain.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GrowDay.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class HabitRecordController : ControllerBase
    {
        protected readonly IHabitRecordService _habitRecordService;
        public HabitRecordController(IHabitRecordService habitRecordService)
        {
            _habitRecordService = habitRecordService;
        }
        [HttpGet("GetHabitRecords")]
        public async Task<IActionResult> GetHabitRecords()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is required.");
            }
            var result = await _habitRecordService.GetHabitRecordByUserAsync(userId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpGet("{habitRecordId}")]
        public async Task<IActionResult> GetHabitRecordById(string habitRecordId)
        {
            if (string.IsNullOrEmpty(habitRecordId))
            {
                return BadRequest("Habit Record ID is required.");
            }
            var result = await _habitRecordService.GetHabitRecordByIdAsync(habitRecordId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpPost("CreateHabitRecord")]
        public async Task<IActionResult> CreateHabitRecord([FromBody] AddHabitRecordDTO addHabitRecordDTO)
        {
            if (addHabitRecordDTO == null)
            {
                return BadRequest("Habit Record data is required.");
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is required.");
            }
            var result = await _habitRecordService.CreateHabitRecordAsync(addHabitRecordDTO);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpDelete("{habitRecordId}")]
        public async Task<IActionResult> DeleteHabitRecord(string habitRecordId)
        {
            if (string.IsNullOrEmpty(habitRecordId))
            {
                return BadRequest("Habit Record ID is required.");
            }
            var result = await _habitRecordService.DeleteHabitRecordAsync(habitRecordId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpPut("{habitRecordId}")]
        public async Task<IActionResult> UpdateHabitRecord(string habitRecordId, [FromBody] UpdateHabitRecordDTO updateHabitRecordDTO)
        {
            if (string.IsNullOrEmpty(habitRecordId) || updateHabitRecordDTO == null)
            {
                return BadRequest("Habit Record ID and data are required.");
            }
            var result = await _habitRecordService.UpdateHabitRecordAsync(habitRecordId, updateHabitRecordDTO);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
    }
}
