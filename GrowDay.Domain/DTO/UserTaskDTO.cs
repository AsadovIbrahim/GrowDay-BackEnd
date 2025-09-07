namespace GrowDay.Domain.DTO
{
    public class UserTaskDTO
    {
        public string UserTaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Points { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string UserHabitId { get; set; }

    }
}
