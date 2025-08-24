using GrowDay.Application.Repositories;
using GrowDay.Application.Services;
using GrowDay.Domain.DTO;
using GrowDay.Domain.Entities.Concretes;
using GrowDay.Domain.Enums;
using GrowDay.Domain.Helpers;
using GrowDay.Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace GrowDay.Persistance.Services
{
    public class NotificationService : INotificationService
    {
        protected readonly IWriteNotificationRepository _writeNotificationRepository;
        protected readonly IReadNotificationRepository _readNotificationRepository;
        protected readonly IWriteUserHabitRepository _writeUserHabitRepository;
        protected readonly IReadUserHabitRepository _readUserHabitRepository;
        protected readonly ILogger<NotificationService> _logger;
        protected readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(IWriteNotificationRepository writeNotificationRepository, ILogger<NotificationService> logger,
            IReadNotificationRepository readNotificationRepository, IHubContext<NotificationHub> hubContext,
            IWriteUserHabitRepository writeUserHabitRepository, IReadUserHabitRepository readUserHabitRepository)
        {
            _logger = logger;
            _writeNotificationRepository = writeNotificationRepository;
            _readNotificationRepository = readNotificationRepository;
            _hubContext = hubContext;
            _writeUserHabitRepository = writeUserHabitRepository;
            _readUserHabitRepository = readUserHabitRepository;
        }

        public async Task<Result> ClearAllNotifications()
        {
            try
            {
                var notifications = await _readNotificationRepository.GetAllAsync();
                if (notifications == null || !notifications.Any())
                {
                    return Result.FailureResult("No notifications found to clear.");
                }
                foreach (var notification in notifications)
                {
                    await _writeNotificationRepository.DeleteAsync(notification);
                }
                return Result.SuccessResult("All notifications cleared successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing notifications");
                return Result.FailureResult("An error occurred while clearing notifications.");
            }
        }

        public async Task<Result<NotificationDTO>> CreateAndSendNotificationAsync(string userHabitId, string userId, string title, string message)
        {
            try
            {
                var notification = new Notification
                {
                    UserId = userId,
                    UserHabitId = userHabitId,
                    Title = title,
                    Message = message,
                    IsRead = false,
                    SentAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                };
                await _writeNotificationRepository.AddAsync(notification);
                var notificationDTO = new NotificationDTO
                {
                    Id = notification.Id,
                    UserHabitId = notification.UserHabitId,
                    UserId = notification.UserId,
                    HabitTitle = notification.Title,
                    Message = notification.Message,
                    IsRead = notification.IsRead,
                    NotificationType = notification.NotificationType,
                    CreatedAt = notification.CreatedAt,
                    SentAt = notification.SentAt
                };

                await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", notificationDTO);
                return Result<NotificationDTO>.SuccessResult(notificationDTO, "Notification created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating notification");
                return Result<NotificationDTO>.FailureResult("An error occurred while creating the notification.");
            }
        }

      

        public async Task<Result> DeleteNotificationAsync(string notificationId)
        {
            try
            {
                var notification = await _readNotificationRepository.GetByIdAsync(notificationId);
                if (notification == null)
                {
                    return Result.FailureResult("Notification not found.");
                }
                await _writeNotificationRepository.DeleteAsync(notification);
                return Result.SuccessResult("Notification deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting notification");
                return Result.FailureResult("An error occurred while deleting the notification.");
            }
        }

        public async Task<Result<NotificationDTO>> GetUserNotificationByIdAsync(string notificationId)
        {
            try
            {
                var notification = await _readNotificationRepository.GetByIdAsync(notificationId);
                if (notification == null)
                {
                    return Result<NotificationDTO>.FailureResult("Notification not found for the specified user.");
                }
                var notificationDTO = new NotificationDTO
                {
                    Id = notification.Id,
                    UserHabitId = notification.UserHabitId,
                    UserId = notification.UserId,
                    HabitTitle = notification.Title,
                    Message = notification.Message,
                    IsRead = notification.IsRead,
                    CreatedAt = notification.CreatedAt,
                    SentAt = notification.SentAt,
                    NotificationType = notification.NotificationType
                };
                return Result<NotificationDTO>.SuccessResult(notificationDTO, "Notification retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving notification by ID");
                return Result<NotificationDTO>.FailureResult("An error occurred while retrieving the notification.");
            }
        }

        public async Task<Result<IEnumerable<NotificationDTO>>> GetUserNotificationsAsync(string userId, int pageIndex = 0, int pageSize = 10)
        {
            try
            {
                var notifications = await _readNotificationRepository.GetNotificationsByUserIdAsync(userId, pageIndex, pageSize);
                if (notifications == null || !notifications.Any())
                {
                    return Result<IEnumerable<NotificationDTO>>.FailureResult("No notifications found for the specified user.");
                }
                var filteredNotifications = notifications.Where(n => n.SentAt != null).ToList();

                var notificationDTOs = notifications.Select(n => new NotificationDTO
                {
                    Id = n.Id,
                    UserHabitId = n.UserHabitId,
                    UserId = n.UserId,
                    HabitTitle = n.Title,
                    Message = n.Message,
                    IsRead = n.IsRead,
                    CreatedAt = n.CreatedAt,
                    SentAt = n.SentAt,
                    NotificationType = n.NotificationType
                }).ToList();
                return Result<IEnumerable<NotificationDTO>>.SuccessResult(notificationDTOs, "Paginated user notifications retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving paginated user notifications");
                return Result<IEnumerable<NotificationDTO>>.FailureResult("An error occurred while retrieving paginated user notifications.");
            }
        }

        public async Task<Result> MarkAsReadAsync(string notificationId)
        {
            try
            {
                var notification = await _readNotificationRepository.GetByIdAsync(notificationId);
                if (notification == null)
                {
                    return Result.FailureResult("Notification not found.");
                }
                if (notification.IsRead)
                {
                    return Result.SuccessResult("Notification is already marked as read.");
                }
                notification.IsRead = true;
                await _writeNotificationRepository.UpdateAsync(notification);
                return Result.SuccessResult("Notification marked as read.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while marking notification as read");
                return Result.FailureResult("An error occurred while marking the notification as read.");
            }
        }

        public async Task<Result> SetNotificationTimeAsync(SetNotificationTimeDTO setNotificationTimeDTO)
        {
            try
            {
                var habit = await _readUserHabitRepository.GetByIdAsync(setNotificationTimeDTO.HabitId);
                if (habit == null)
                {
                    return Result.FailureResult("Habit not found.");
                }
                habit.NotificationTime = setNotificationTimeDTO.NotificationTime;
                habit.DurationInMinutes = setNotificationTimeDTO.DurationInMinutes;
                await _writeUserHabitRepository.UpdateAsync(habit);
                return Result.SuccessResult("Notification time set successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while setting notification time");
                return Result.FailureResult("An error occurred while setting the notification time.");
            }
        }
    }
}
