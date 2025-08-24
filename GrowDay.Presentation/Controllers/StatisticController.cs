using GrowDay.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GrowDay.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class StatisticController : ControllerBase
    {
        protected readonly IStatisticService _statisticService;
        public StatisticController(IStatisticService statisticService)
        {
            _statisticService = statisticService;
        }
        [HttpGet("monthly")]
        public async Task<IActionResult> GetMonthlyStatistics(int year, int month)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _statisticService.GetMonthlyStatisticsAsync(userId!, year, month);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpGet("weekly")]
        public async Task<IActionResult> GetWeeklyStatistics(DateTime weekStart)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _statisticService.GetWeeklyStatisticAsync(userId!, weekStart);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
        [HttpGet("custom")]
        public async Task<IActionResult> GetCustomStatistics(DateTime startDate, DateTime endDate)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _statisticService.CalculateStatisticsAsync(userId!, startDate, endDate);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
    }
}
