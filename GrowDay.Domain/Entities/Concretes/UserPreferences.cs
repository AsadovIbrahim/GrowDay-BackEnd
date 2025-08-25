using GrowDay.Domain.Entities.Common;
using GrowDay.Domain.Enums;

namespace GrowDay.Domain.Entities.Concretes
{
    public class UserPreferences : BaseEntity
    {
        public string UserId { get; set; }
        public TimeSpan WakeUpTime { get; set; }
        public TimeSpan SleepTime { get; set; }
        public ProcrastinateFrequency ProcrestinateFrequency { get; set; }
        public FocusDifficulty FocusDifficulty { get; set; }
        public MotivationalFactors MotivationalFactors { get; set; }

        // Navigation Property
        public virtual User? User { get; set; }

    }
}
