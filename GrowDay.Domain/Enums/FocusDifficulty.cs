using System.Text.Json.Serialization;

namespace GrowDay.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum FocusDifficulty
    {
        Constantly,
        Occasionally,
        Rarely,
        Never
    }
}
