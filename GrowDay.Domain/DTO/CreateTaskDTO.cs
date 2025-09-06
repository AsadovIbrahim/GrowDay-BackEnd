namespace GrowDay.Domain.DTO
{
    public class CreateTaskDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Points { get; set; }
        public bool IsActive { get; set; } = true;

    }
}
    