namespace GrowDay.Domain.DTO
{
    public class HabitRecordDTO
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public bool IsCompleted { get; set; }
        public string Note { get; set; }


        public string HabitId { get; set; }
    }
}
