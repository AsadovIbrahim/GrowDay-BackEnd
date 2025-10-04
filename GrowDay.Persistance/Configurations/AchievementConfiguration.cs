using GrowDay.Domain.Entities.Concretes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GrowDay.Persistance.Configurations
{
    public class AchievementConfiguration : IEntityTypeConfiguration<Achievement>
    {
        public void Configure(EntityTypeBuilder<Achievement> builder)
        {
            builder.HasOne(a => a.Habit)
                   .WithMany(h => h.Achievements)
                   .HasForeignKey(a => a.HabitId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
