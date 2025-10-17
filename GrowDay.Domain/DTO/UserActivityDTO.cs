using GrowDay.Domain.Enums;

namespace GrowDay.Domain.DTO
{
    public class UserActivityDTO
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ActivityType ActivityType { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
