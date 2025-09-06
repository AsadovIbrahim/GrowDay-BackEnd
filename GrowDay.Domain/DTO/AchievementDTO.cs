namespace GrowDay.Domain.DTO
{
    public class AchievementDTO
    {
        public string AchievementId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int PointsRequired { get; set; } 
        public int StreakRequired { get; set; }
        public int TaskCompletionRequired { get; set; }
    }
}
