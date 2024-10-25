using GitHubActionDemo.Entity;
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
                      .HasMaxLength(20); // Example of property configuration
            });
        }
    }
}
