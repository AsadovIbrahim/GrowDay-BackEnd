namespace GrowDay.Domain.DTO
{
    public class UpdateHabitRecordDTO
    {
        public DateTime Date { get; set; }
        public bool IsCompleted { get; set; }
        public string Note { get; set; }


        public string UserHabitId { get; set; }
    }
}
