namespace GrowDay.Domain.DTO
{
    public class WeeklyHabitProgressDTO
    {
        public DateTime Date { get; set; }
        public string Day { get; set; }
        public string DayAbbreviation { get; set; }
        public int DayNumber { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsToday { get; set; }

    }
}
