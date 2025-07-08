using Domain.Entities.Roles;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations
{
    internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(e => e.UserId);

            builder.Property(e => e.UserId)
                      .ValueGeneratedOnAdd();

            builder.Property(e => e.Email)
                      .IsRequired()
                      .HasMaxLength(50);

            builder.Property(e => e.PasswordHash)
                      .IsRequired();

            builder.Property(e => e.UserName)
                      .IsRequired()
                      .HasMaxLength(20);

            builder.HasMany(e => e.Roles)
                 .WithMany(u => u.Users)
                 .UsingEntity<UserRole>();
        }
    }
}
