using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Common;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Data.Config
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(x => x.Id);

            builder.OwnsOne<Email>(u => u.Email, email =>
            {
                email.Property(e => e.Value)
                .HasColumnName("Email")
                .IsRequired();

                email.HasIndex(e => e.Value).IsUnique();
            });

            builder.OwnsMany(u => u.RefreshTokens, rt =>
            {
                rt.WithOwner().HasForeignKey("UserId");

                rt.ToTable("UserRefreshTokens");
            });
        }
    }
}
