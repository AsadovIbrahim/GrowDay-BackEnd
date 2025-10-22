using GrowDay.Application.Services;
using GrowDay.Domain.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GrowDay.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        protected readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("CreateNotification")]
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationDTO createNotificationDTO)
        {
            if (createNotificationDTO == null)
            {
                return BadRequest("Invalid notification data.");
            }

            var result = await _notificationService.CreateAndSendNotificationAsync(
                createNotificationDTO.HabitId!,
                createNotificationDTO.UserId,
                createNotificationDTO.Title,
                createNotificationDTO.Message,
                createNotificationDTO.NotificationType);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }

        [Authorize(Roles = "User")]
        [HttpGet("{notificationId}")]
        public async Task<IActionResult> GetNotificationById(string notificationId)
        {
            if (string.IsNullOrEmpty(notificationId))
            {
                return BadRequest("Notification ID cannot be null or empty.");
            }
            var result = await _notificationService.GetUserNotificationByIdAsync(notificationId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);
        }

        [Authorize(Roles = "User")]
        [HttpGet("getnotification")]
        public async Task<IActionResult> GetNotificationsByUser([FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 10)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID cannot be null or empty.");
            }

            var result = await _notificationService.GetUserNotificationsAsync(userId, pageIndex, pageSize);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Data);
        }

        [Authorize(Roles = "User")]
        [HttpGet("unreadnotifications")]
        public async Task<IActionResult> GetUnreadNotificationsByUser([FromQuery] int pageIndex = 0, [FromQuery] int pageSize = 10)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID cannot be null or empty.");
            }
            var result = await _notificationService.GetUnreadUserNotificationsAsync(userId, pageIndex, pageSize);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);
        }

        [Authorize(Roles = "User")]
        [HttpPut("read/{notificationId}")]
        public async Task<IActionResult> MarkNotificationAsRead(string notificationId)
        {
            if (string.IsNullOrEmpty(notificationId))
            {
                return BadRequest("Notification ID cannot be null or empty.");
            }

            var result = await _notificationService.MarkAsReadAsync(notificationId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }

        [Authorize(Roles ="User")]
        [HttpPut("readall")]
        public async Task<IActionResult> MarkAllNotificationsAsRead()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID cannot be null or empty.");
            }
            var result = await _notificationService.MarkAllAsReadAsync(userId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        [Authorize(Roles = "User")]
        [HttpPut("SetNotificationTime")]
        public async Task<IActionResult> SetNotificationTime([FromBody] SetNotificationTimeDTO setNotificationTimeDTO)
        {
            if (setNotificationTimeDTO == null || string.IsNullOrEmpty(setNotificationTimeDTO.HabitId))
            {
                return BadRequest("Habit ID cannot be null or empty.");
            }
            var result = await _notificationService.SetNotificationTimeAsync(setNotificationTimeDTO);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        [Authorize(Roles = "User")]
        [HttpDelete("{notificationId}")]
        public async Task<IActionResult> DeleteNotification(string notificationId)
        {
            if (string.IsNullOrEmpty(notificationId))
            {
                return BadRequest("Notification ID cannot be null or empty.");
            }

            var result = await _notificationService.DeleteNotificationAsync(notificationId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }
        [Authorize(Roles = "User")]
        [HttpDelete("ClearAllNotifications")]
        public async Task<IActionResult> ClearAllNotifications()
        {
            var result = await _notificationService.ClearAllNotifications();
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
    }
}
