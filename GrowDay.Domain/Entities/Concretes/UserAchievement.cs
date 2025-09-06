using GrowDay.Domain.Entities.Common;

namespace GrowDay.Domain.Entities.Concretes
{
    public class UserAchievement: BaseEntity
    {
        public string UserId { get; set; }
        public virtual User User { get; set; }

        public string AchievementId { get; set; }
        public virtual Achievement Achievement { get; set; }

        public DateTime EarnedAt { get; set; } = DateTime.UtcNow;

    }
}
