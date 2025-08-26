using System.Text.Json.Serialization;

namespace GrowDay.Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum HabitCriteria
    {
        None,
        MorningPerson,       
        NightOwl,            
        HighProcrastination, 
        LowProcrastination,  
        FocusDifficultyHigh, 
        LowMotivation,       
        DigitalDistractions  
    }
}
