namespace GrowDay.Domain.DTO
{
    public class AddHabitRecordDTO
    {
        public string UserHabitId { get; set; }
        public DateTime Date { get; set; }
        public bool IsCompleted { get; set; }
        public string Note { get; set; }
    }
}
