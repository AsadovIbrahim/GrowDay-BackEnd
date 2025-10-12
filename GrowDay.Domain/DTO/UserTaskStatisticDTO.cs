namespace GrowDay.Domain.DTO
{
    public class UserTaskStatisticDTO
    {
        public string UserTaskId { get; set; }
        public string Title { get; set; }
        public int Point { get; set; }
        public DateTime? CompletedAt { get; set; }

        public int RequiredPoints { get; set; }
        public int TotalRequiredCompletions { get; set; }
        public int TotalPoints { get; set; }
        public int TotalTasksCompleted { get; set; }
        public int CurrentStreak { get; set; }
        public int LongestStreak { get; set; }
        public double CurrentValue { get; set; }
        public double TargettValue { get; set; }
        public double CompletionPercentage { get; set; }
    }
}
