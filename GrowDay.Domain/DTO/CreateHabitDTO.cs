using GrowDay.Domain.Enums;

namespace GrowDay.Domain.DTO
{
    public class CreateHabitDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }

        public int? TargetValue { get; set; }
        public int? IncrementValue { get; set; }
        public string? Unit { get; set; }

        public HabitFrequency Frequency { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
