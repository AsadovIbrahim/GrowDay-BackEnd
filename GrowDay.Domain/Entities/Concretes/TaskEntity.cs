using GrowDay.Domain.Entities.Common;

namespace GrowDay.Domain.Entities.Concretes
{
    public class TaskEntity : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Points { get; set; }
        public bool IsActive { get; set; } = true;

        public int? TotalRequiredCompletions { get; set; }
        public int? RequiredPoints { get; set; }
        public string? HabitId { get; set; }
        public virtual Habit Habit { get; set; }
        public virtual ICollection<UserTask> UserTasks { get; set; }
    }
}
