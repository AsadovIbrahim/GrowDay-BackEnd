using GrowDay.Domain.Entities.Concretes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GrowDay.Persistance.Configurations
{
    public class UserTokenConfiuration : IEntityTypeConfiguration<UserToken>
    {
        public void Configure(EntityTypeBuilder<UserToken> builder)
        {
            builder.HasKey(ut => ut.Id);

            builder.Property(ut => ut.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(ut => ut.Token)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(ut => ut.ExpireTime)
                .IsRequired();

            builder.HasOne(ut => ut.User)
                .WithMany(u=>u.UserTokens) 
                .HasForeignKey(ut => ut.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
