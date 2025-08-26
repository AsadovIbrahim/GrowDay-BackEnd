namespace GrowDay.Domain.DTO
{
    public class AddSuggestedHabitDTO
    {
        public string SuggestedHabitId { get; set; }
        public TimeSpan? NotificationTime { get; set; }
        public int? DurationInMinutes { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }


    }
}
