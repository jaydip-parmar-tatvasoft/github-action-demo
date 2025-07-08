using Domain.Entities.Roles;
using Domain.Enums;
using GitHubActionDemo.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations
{
    internal sealed class RoleEnumConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");
            builder.HasKey(x => x.Id);
            builder
                .HasMany(x => x.Permissions)
                .WithMany(x => x.Roles)
                .UsingEntity<RolePermission>(rp => rp.HasData(
                        Create(
                            new Role((int)RoleType.Admin, RoleType.Admin.ToString()), Permission.ReadMember)
                        ));

            IEnumerable<Role> roles = Enum.GetValues<RoleType>()
                                          .Select(p => new Role((int)p, p.ToString()));
            builder.HasData(roles);
        }

        private static RolePermission Create(Role role, Domain.Enums.Permission permission)
        {
            return new RolePermission
            {
                RoleId = role.Id,
                PermissionId = (int)permission
            };
        }
    }
}
