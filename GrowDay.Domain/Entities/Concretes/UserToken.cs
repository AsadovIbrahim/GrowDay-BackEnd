using GrowDay.Domain.Entities.Common;

namespace GrowDay.Domain.Entities.Concretes
{
    public class UserToken : BaseEntity
    {
        public string Name { get; set; }
        public string Token { get; set; }
        public DateTime ExpireTime { get; set; }

        // Foreign Key

        public string UserId { get; set; }

        // Navigation

        public virtual User? User { get; set; }
    }
}
