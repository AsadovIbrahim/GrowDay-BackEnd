using GrowDay.Domain.Entities.Common;
using GrowDay.Domain.Enums;

namespace GrowDay.Domain.Entities.Concretes
{
    public class Notification : BaseEntity
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; } = false;

        public DateTime? SentAt { get; set; }
        public NotificationType NotificationType { get; set; }

        // Foreign Keys
        public string UserId { get; set; }
        public string? UserHabitId { get; set; }



        public virtual User User { get; set; }
        public virtual UserHabit? UserHabit { get; set; }

    }
}
