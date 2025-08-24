using GrowDay.Domain.Entities.Concretes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GrowDay.Persistance.Configurations
{
    public class UserHabitConfiguration : IEntityTypeConfiguration<UserHabit>
    {
        public void Configure(EntityTypeBuilder<UserHabit> builder)
        {
            builder.HasKey(uh => uh.Id);

            builder.HasOne(uh => uh.User)
                .WithMany(u => u.UserHabits)
                .HasForeignKey(uh => uh.UserId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasOne(uh => uh.Habit)
                .WithMany(h => h.UserHabits)
                .HasForeignKey(uh => uh.HabitId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
