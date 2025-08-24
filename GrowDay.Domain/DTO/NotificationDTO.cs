using GrowDay.Domain.Enums;

namespace GrowDay.Domain.DTO
{
    public class NotificationDTO
    {
        public string Id { get; set; }
        public string HabitTitle { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? SentAt { get; set; }
        public NotificationType NotificationType { get; set; }
        public string UserId { get; set; }
        public string? UserHabitId { get; set; }
    }
}
