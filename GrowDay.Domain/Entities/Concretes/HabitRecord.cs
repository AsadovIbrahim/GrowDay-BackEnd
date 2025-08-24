using GrowDay.Domain.Entities.Common;

namespace GrowDay.Domain.Entities.Concretes
{
    public class HabitRecord : BaseEntity
    {
        public DateTime Date { get; set; }
        public bool IsCompleted { get; set; }
        public string? Note { get; set; }


        //Foreign Key
        public string HabitId { get; set; }
        public virtual Habit? Habit { get; set; }
    }
}
