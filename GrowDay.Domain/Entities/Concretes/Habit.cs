using GrowDay.Domain.Entities.Common;
using GrowDay.Domain.Enums;

namespace GrowDay.Domain.Entities.Concretes
{
    public class Habit : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public HabitFrequency Frequency { get; set; }
        public bool IsActive { get; set; } = true;


        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public bool IsGlobal { get; set; } = false;
        public string? CreatedByUserId { get; set; }

        public int? TargetValue { get; set; }
        public int? IncrementValue { get; set; }
        public string? Unit { get; set; }

        //Navigation Property
        public virtual ICollection<UserHabit>? UserHabits { get; set; }
        public virtual ICollection<TaskEntity>? Tasks { get; set; }
        public virtual ICollection<Achievement>? Achievements { get; set; }


        }
}
