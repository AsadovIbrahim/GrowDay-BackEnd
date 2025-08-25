using System.Text.Json.Serialization;

namespace GrowDay.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ProcrastinateFrequency
    {
        Always,
        Sometimes,
        Rarely,
        Never
    }
}
