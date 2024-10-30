using GitHubActionDemo.Entities;
using GitHubActionDemo.Extensions;
using GitHubActionDemo.Seeds;
using GitHubActionDemo.Service;
using Microsoft.EntityFrameworkCore;

namespace GitHubActionDemo.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<User> users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users", "public");

                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId)
                      .HasColumnName("user_id")
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.Email)
                      .HasColumnName("email")
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(e => e.PasswordHash)
                      .HasColumnName("pasword_hash")
                      .IsRequired();

                entity.Property(e => e.UserId)
                      .HasColumnName("user_id")
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.UserName)
                      .HasColumnName("user_name")
                      .IsRequired()
                      .HasMaxLength(20);

                entity.Property(e => e.RefreshToken)
                      .HasColumnName("refresh_token");

                entity.Property(e => e.RefreshTokenExpireOnUtc)
                      .HasColumnName("refresh_token_expireon_utc");

                entity.HasMany(e => e.Roles)
                     .WithMany()
                     .UsingEntity<RoleUser>();
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("roles", "public");

                entity.HasKey(x => x.Id);

                entity.Property(e => e.Id)
                      .HasColumnName("role_id");

                entity.Property(e => e.Name)
                      .HasColumnName("role_name");

                entity.HasMany(e => e.Permissions)
                   .WithMany()
                   .UsingEntity<RolePermission>();

                entity.HasMany(e => e.Users)
                      .WithMany()
                      .UsingEntity<RoleUser>();

                entity.HasData(Role.GetAll());
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.ToTable("permissions", "public");

                entity.HasKey(x => x.Id);

                entity.Property(e => e.Id)
                      .HasColumnName("permission_id");

                var permissions = Enum.GetValues<Enums.Permission>().Select(x => new Permission
                {
                    Id = (int)x,
                    Name = x.GetEnumDescription()
                });

                entity.HasData(permissions);
            });

            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.ToTable("role_permissions", "public");

                entity.HasKey(x => new { x.RoleId, x.PermissionId });

                entity.Property(e => e.PermissionId)
                      .HasColumnName("permission_id");

                entity.Property(e => e.RoleId)
                      .HasColumnName("role_id");

                entity.HasData(SeedRolePermission.Create(Role.Registered, Enums.Permission.ViewUser));
            });

            modelBuilder.Entity<RoleUser>(entity =>
            {
                entity.ToTable("role_users", "public");

                entity.HasKey(x => new { x.RoleId, x.UserId });

                entity.Property(e => e.RoleId)
                      .HasColumnName("role_id");

                entity.Property(e => e.UserId)
                      .HasColumnName("user_id");
            });
        }
    }
}
