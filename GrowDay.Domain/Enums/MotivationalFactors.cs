using System.Text.Json.Serialization;

namespace GrowDay.Domain.Enums
{
    [Flags]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum MotivationalFactors
    {
        None = 0,
        LackOfMotivation = 1,
        WorkOverload = 2,
        ClutteredEnvironment = 4,
        DigitalDistractions = 8,
        LackOfTimeManagement = 16,
        Other = 32

    }
}
