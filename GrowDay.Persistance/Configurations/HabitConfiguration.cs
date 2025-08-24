using GrowDay.Domain.Entities.Concretes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GrowDay.Persistance.Configurations
{
    public class HabitConfiguration : IEntityTypeConfiguration<Habit>
    {
        public void Configure(EntityTypeBuilder<Habit> builder)
        {
            builder.HasKey(h => h.Id);

            builder.Property(h => h.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(h => h.Description)
                .HasMaxLength(500);


            builder.Property(h => h.Frequency)
                .HasConversion<string>()
                .IsRequired();

            builder.HasMany(h=>h.HabitRecords)
                .WithOne(hr => hr.Habit)
                .HasForeignKey(hr => hr.HabitId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasMany(h => h.UserHabits)
                .WithOne(uh => uh.Habit)
                .HasForeignKey(uh => uh.HabitId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
