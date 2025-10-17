using System.Text.Json.Serialization;

namespace GrowDay.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ActivityType
    {
        PointsEarned,
        TaskCompleted,
        HabitCompleted,
        AchievementEarned,
        StreakBroken
    }
}
