using GrowDay.Domain.Entities.Common;

namespace GrowDay.Domain.Entities.Concretes
{
    public class Achievement : BaseEntity
    {
        public string? HabitId { get; set; }
        public virtual Habit? Habit { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int PointsRequired { get; set; }
        public int? StreakRequired { get; set; }
        public int? TaskCompletionRequired { get; set; }
        public virtual ICollection<UserAchievement>? UserAchievements { get; set; }

    }
}
