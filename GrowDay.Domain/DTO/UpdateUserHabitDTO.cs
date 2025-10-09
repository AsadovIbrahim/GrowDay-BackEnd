using GrowDay.Domain.Enums;

namespace GrowDay.Domain.DTO
{
    public class UpdateUserHabitDTO
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public HabitFrequency? Frequency { get; set; }

        public int? TargetValue { get; set; }
        public int? IncrementValue { get; set; }
        public string? Unit { get; set; }

        public TimeSpan? NotificationTime { get; set; } 
        public int? DurationInMinutes { get; set; }   
    }
}
