using GrowDay.Domain.Enums;

namespace GrowDay.Domain.DTO
{
    public class CreateNotificationDTO
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string UserId { get; set; }
        public string? HabitId { get; set; }
        public NotificationType NotificationType { get; set; }
        public DateTime? SentAt { get; set; }
    }
}
