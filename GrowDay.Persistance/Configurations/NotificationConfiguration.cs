using GrowDay.Domain.Entities.Concretes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GrowDay.Persistance.Configurations
{
    public class NotificationConfiguration:IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasKey(n => n.Id);

            builder.Property(n => n.Title)
                .IsRequired()
                .HasMaxLength(100); 

            builder.Property(n => n.Message)
                .IsRequired()
                .HasMaxLength(500); 

            builder.Property(n => n.SentAt)
                .IsRequired(false); 

            builder.Property(n => n.IsRead)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(n => n.NotificationType)
                .IsRequired()
                .HasConversion<string>(); 

          
            builder.HasOne(n => n.UserHabit)
                .WithMany(uh=>uh.Notifications)
                .HasForeignKey(n => n.UserHabitId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(n => n.User)
                .WithMany(u=>u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);


        }
    }
    
}
