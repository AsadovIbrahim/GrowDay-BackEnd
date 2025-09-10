using GrowDay.Domain.Entities.Common;

namespace GrowDay.Domain.Entities.Concretes
{
    public class UserTask : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Points { get; set; }
        public bool IsCompleted { get; set; } = false;
        public DateTime? CompletedAt { get; set; }

        public int? TotalRequiredCompletions { get; set; }
        public int? RequiredPoints { get; set; }
        public int? StreakRequired { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }

        public string? UserHabitId { get; set; }
        public virtual UserHabit? UserHabit { get; set; }


        public string TaskId { get; set; }
        public virtual TaskEntity Task { get; set; }

        public virtual ICollection<UserTaskCompletion> Completions { get; set; }
    }
}
