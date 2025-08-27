using GrowDay.Domain.Entities.Abstracts;
using Microsoft.AspNetCore.Identity;

namespace GrowDay.Domain.Entities.Concretes
{
    public class User : IdentityUser, IBaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        //Audit fields
        public bool IsDeleted { get; set; } = false;
        public bool FirstTimeLogin { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastModifiedAt { get; set; } = DateTime.UtcNow;


        //Navigation
        public virtual ICollection<UserHabit>? UserHabits { get; set; }
        public virtual ICollection<UserToken>? UserTokens { get; set; }
        public virtual ICollection<Notification>? Notifications { get; set; }
        public virtual ICollection<Statistic>? Statistics { get; set; }
        public virtual ICollection<UserPreferences>? UserPreferences { get; set; }

    }

}
