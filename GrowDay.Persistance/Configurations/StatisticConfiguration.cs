using Microsoft.EntityFrameworkCore;
using GrowDay.Domain.Entities.Concretes;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GrowDay.Persistance.Configurations
{
    public class StatisticConfiguration : IEntityTypeConfiguration<Statistic>
    {
        public void Configure(EntityTypeBuilder<Statistic> builder)
        {
            builder.HasKey(s => s.Id);
         
            builder.Property(s => s.CompletedCount)
                .IsRequired();

            builder.Property(s => s.MissedCount)
                .IsRequired();

            builder.Property(s => s.PeriodStart)
                .IsRequired();
            
            builder.Property(s => s.PeriodEnd)
                .IsRequired();
            
            builder.Property(s => s.PeriodType)
                .IsRequired()
                .HasConversion<string>();
        }
    }
}
