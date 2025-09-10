using GrowDay.Domain.Entities.Common;

namespace GrowDay.Domain.Entities.Concretes
{
    public class UserTaskCompletion:BaseEntity
    {
        public string UserTaskId { get; set; }
        public virtual UserTask UserTask { get; set; }

        public int Points { get; set; }
        public int CurrentStreak { get; set; }
        public int LongestStreak { get; set; }
        public DateTime CompletedAt { get; set; }=DateTime.UtcNow;


    }
}
