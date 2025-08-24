using GrowDay.Domain.Enums;

namespace GrowDay.Domain.DTO
{
    public class HabitDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }


        public HabitFrequency Frequency { get; set; }
        public bool IsActive { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

    }
}
