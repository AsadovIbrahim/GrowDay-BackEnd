using GrowDay.Domain.Enums;

namespace GrowDay.Domain.DTO
{
    public class UserHabitDTO
    {
        public string UserHabitId { get; set; }
        public string? UserId { get; set; }
        public string? HabitId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public HabitFrequency? Frequency { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int CurrentStreak { get; set; }
        public int LongestStreak { get; set; }

        public double? CurrentValue { get; set; }
        public double? TargetValue { get; set; }
        public double? IncrementValue { get; set; }
        public string? Unit { get; set; }
        public double? ProgressPercentage { get; set; }

        public DateTime? LastCompletedDate { get; set; }

        public TimeSpan? NotificationTime { get; set; }
        public int? DurationInMinutes { get; set; }

    }
}
