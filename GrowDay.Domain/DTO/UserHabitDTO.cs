using GrowDay.Domain.Enums;

namespace GrowDay.Domain.DTO
{
    public class UserHabitDTO
    {
        public string Id { get; set; }
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

        public DateTime? LastCompletedDate { get; set; }

        public TimeSpan? NotificationTime { get; set; }
        public int? DurationInMinutes { get; set; }

    }
}
