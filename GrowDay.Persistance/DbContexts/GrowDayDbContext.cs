using Microsoft.EntityFrameworkCore;
using GrowDay.Domain.Entities.Concretes;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace GrowDay.Persistance.DbContexts
{
    public class GrowDayDbContext : IdentityDbContext<User>
    {
        public GrowDayDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(GrowDayDbContext).Assembly);

            base.OnModelCreating(builder);
        }


        //Tables

        public virtual DbSet<Habit> Habits { get; set; }
        public virtual DbSet<HabitRecord> HabitRecords { get; set; }
        public virtual DbSet<Statistic> Statistics { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<UserHabit> UserHabits { get; set; }
        public virtual DbSet<UserPreferences> UserPreferences { get; set; }
        public virtual DbSet<SuggestedHabit> SuggestedHabits { get; set; }
        public virtual DbSet<UserToken> UserTokens { get; set; }
        public virtual DbSet<JobExecutionLog> JobExecutionLogs { get; set; }

    }
}
