namespace GrowDay.Domain.DTO
{
    public class TaskDTO
    {
        public string HabitId { get; set; }
        public string TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Points { get; set; }
        public int? TotalRequiredCompletions { get; set; }
        public int? RequiredPoints { get; set; }
        public int? StreakRequired { get; set; }
        public bool IsActive { get; set; }

    }
}
