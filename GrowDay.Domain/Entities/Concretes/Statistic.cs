using GrowDay.Domain.Entities.Common;
using GrowDay.Domain.Enums;

namespace GrowDay.Domain.Entities.Concretes
{
    public class Statistic:BaseEntity
    {
        public int CompletedCount { get; set; }
        public int MissedCount{ get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public StatisticPeriodType PeriodType { get; set; }

        //Foreign Key
        public string UserId { get; set; }
        //Navigation
        public virtual User User { get; set; }
    }
}
