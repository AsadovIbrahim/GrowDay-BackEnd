using GrowDay.Domain.Enums;

namespace GrowDay.Domain.DTO
{
    public class UpdateUserPreferenceDTO
    {
        public TimeSpan? WakeUpTime { get; set; }
        public TimeSpan? SleepTime { get; set; }
        public ProcrastinateFrequency? ProcrestinateFrequency { get; set; }
        public FocusDifficulty? FocusDifficulty { get; set; }
        public MotivationalFactors? MotivationalFactors { get; set; }
    }
}
