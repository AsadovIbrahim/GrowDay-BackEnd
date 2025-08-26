using GrowDay.Domain.Enums;
using GrowDay.Domain.Entities.Common;

namespace GrowDay.Domain.Entities.Concretes
{
    public class SuggestedHabit : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public HabitFrequency? Frequency { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsSelected { get; set; } = false;
        public HabitCriteria Criteria { get; set; }

        public TimeSpan? NotificationTime { get; set; }
        public int? DurationInMinutes { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }


        // Navigation Properties
        public string? UserHabitId { get; set; }
        public virtual UserHabit? UserHabit { get; set; }
    }
}
