using GrowDay.Domain.Entities.Common;
using GrowDay.Domain.Enums;

namespace GrowDay.Domain.Entities.Concretes
{
    public class UserActivity:BaseEntity
    {
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ActivityType ActivityType { get; set; }
    }
}
