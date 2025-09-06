namespace GrowDay.Domain.DTO
{
    public class TaskDTO
    {
        public string TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Points { get; set; }
        public bool IsActive { get; set; }

    }
}
