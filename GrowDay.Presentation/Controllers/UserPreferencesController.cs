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
    public class UserPreferencesController : ControllerBase
    {
        protected readonly IUserPreferencesService _userPreferencesService;

        public UserPreferencesController(IUserPreferencesService userPreferencesService)
        {
            _userPreferencesService = userPreferencesService;
        }

        [HttpGet("GetUserPreferences")]
        public async Task<IActionResult> GetUserPreferences()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { Message = "User is not authenticated." });
            }
            var result = await _userPreferencesService.GetUserPreferencesAsync(userId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpPost("CreateUserPreferences")]
        public async Task<IActionResult> CreateUserPreferences([FromBody] CreateUserPreferenceDTO dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { Message = "User is not authenticated." });
            }
            var result = await _userPreferencesService.CreateUserPreferencesAsync(userId, dto);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpPut("UpdateUserPreferences")]
        public async Task<IActionResult> UpdateUserPreferences([FromBody] UpdateUserPreferenceDTO dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { Message = "User is not authenticated." });
            }
            var result = await _userPreferencesService.UpdateUserPreferencesAsync(userId, dto);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
    }
}
