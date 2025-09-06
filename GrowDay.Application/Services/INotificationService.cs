using GrowDay.Domain.DTO;
using GrowDay.Domain.Enums;
using GrowDay.Domain.Helpers;

namespace GrowDay.Application.Services
{
    public interface INotificationService
    {
        Task<Result<NotificationDTO>> CreateAndSendNotificationAsync(string habitId, string userId, string title, string message,NotificationType notificationType);
        Task<Result<IEnumerable<NotificationDTO>>> GetUserNotificationsAsync(string userId, int pageIndex = 0, int pageSize = 10);
        Task<Result<NotificationDTO>>GetUserNotificationByIdAsync(string notificationId);
        Task<Result> MarkAsReadAsync(string notificationId);
        Task<Result> DeleteNotificationAsync(string notificationId);

        Task<Result> SetNotificationTimeAsync(SetNotificationTimeDTO setNotificationTimeDTO);
        Task<Result> ClearAllNotifications();


    }
}
