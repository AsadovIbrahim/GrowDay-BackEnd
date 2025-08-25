using System.Text.Json.Serialization;

namespace GrowDay.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum StatisticPeriodType
    {
        Daily,
        Weekly,
        Monthly,
        Custom
    }
}
