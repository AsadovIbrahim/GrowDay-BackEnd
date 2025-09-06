namespace GrowDay.Domain.DTO
{
    public class UserAchievementDTO
    {
        public string UserAchievementId { get; set; }
        public string AchievementId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime EarnedAt { get; set; }
    }
}
