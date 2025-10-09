using GrowDay.Domain.Entities.Common;
using GrowDay.Domain.Enums;

namespace GrowDay.Domain.Entities.Concretes
{
    public class UserHabit : BaseEntity
    {
        public string UserId { get; set; }
        public string? HabitId { get; set; }

        public string? Title { get; set; }
        public string? Description { get; set; }
        public HabitFrequency? Frequency { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; } = true;

        public int CurrentStreak { get; set; }
        public int LongestStreak { get; set; }
        public DateTime? LastCompletedDate { get; set; }

        public int? CurrentValue { get; set; }
        public int? TargetValue { get; set; }
        public int? IncrementValue { get; set; }
        public string? Unit { get; set; }

        public TimeSpan? NotificationTime { get; set; }
        public int? DurationInMinutes { get; set; }

        //Navigation Properties
        public virtual User? User { get; set; }
        public virtual Habit? Habit { get; set; }

        public virtual ICollection<SuggestedHabit>? SuggestedHabits { get; set; }
        public virtual ICollection<Notification>? Notifications { get; set; }
        public virtual ICollection<HabitRecord>? HabitRecords { get; set; }
        public virtual ICollection<UserTask>? UserTasks { get; set; }

        }
}
