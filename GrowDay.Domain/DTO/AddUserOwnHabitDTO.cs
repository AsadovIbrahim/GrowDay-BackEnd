using GrowDay.Domain.Enums;

namespace GrowDay.Domain.DTO
{
    public class AddUserOwnHabitDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public HabitFrequency Frequency { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }


        public double? TargetValue { get; set; }
        public double? IncrementValue { get; set; }
        public string? Unit { get; set; }

        public TimeSpan? NotificationTime { get; set; } 
        public int? DurationInMinutes { get; set; }     
    }
}
