using GrowDay.Domain.Enums;

namespace GrowDay.Domain.DTO
{
    public class AddUserHabitDTO
    {
        public string HabitId { get; set; }
        public TimeSpan? NotificationTime { get; set; } 
        public int? DurationInMinutes { get; set; }

        public HabitFrequency? Frequency { get; set; }
    }
}
