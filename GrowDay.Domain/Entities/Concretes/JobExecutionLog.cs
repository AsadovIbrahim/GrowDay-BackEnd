using GrowDay.Domain.Entities.Common;

namespace GrowDay.Domain.Entities.Concretes
{
    public class JobExecutionLog:BaseEntity
    {
        public string JobName { get; set; } = default!;
        public DateTime LastRunDate { get; set; }
    }
}
