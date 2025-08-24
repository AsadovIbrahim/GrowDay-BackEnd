using GrowDay.Domain.Entities.Concretes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GrowDay.Persistance.Configurations
{
    public class HabitRecordConfiguration : IEntityTypeConfiguration<HabitRecord>
    {
        public void Configure(EntityTypeBuilder<HabitRecord> builder)
        {
            builder.HasKey(hr => hr.Id);

            builder.Property(hr => hr.Note)
                .HasMaxLength(500);

            builder.Property(hr => hr.Date)
                .IsRequired();

            

        }
    }
}
