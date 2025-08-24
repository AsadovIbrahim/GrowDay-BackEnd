using GrowDay.Domain.Enums;

namespace GrowDay.Domain.DTO
{
    public class StatisticDTO
    {
        public StatisticPeriodType PeriodType { get; set; }
        public int CompletedCount { get; set; }
        public int MissedCount { get; set; }
        public double CompletionRate { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
    }
}
