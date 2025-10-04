using GrowDay.Domain.Entities.Concretes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GrowDay.Persistance.Configurations
{
    public class TaskConfiguration : IEntityTypeConfiguration<TaskEntity>
    {
        public void Configure(EntityTypeBuilder<TaskEntity> builder)
        {
            builder.HasOne(t => t.Habit)
                   .WithMany(h => h.Tasks)
                   .HasForeignKey(t => t.HabitId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
