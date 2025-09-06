namespace GrowDay.Domain.DTO
{
    public class CreateAchievementDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int PointsRequired { get; set; }
        public int StreakRequired { get; set; }
        public int TaskCompletionRequired { get; set; }
        public bool IsActive { get; set; } = true;

    }
}
