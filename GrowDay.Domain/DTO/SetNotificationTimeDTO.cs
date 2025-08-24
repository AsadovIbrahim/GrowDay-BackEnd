namespace GrowDay.Domain.DTO
{
    public class SetNotificationTimeDTO
    {
        public string HabitId { get; set; }
        public TimeSpan NotificationTime { get; set; }
        public int DurationInMinutes { get; set; }
    }
}
