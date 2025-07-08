using Domain.Entities.Roles;
using GitHubActionDemo.Entities;

namespace Domain.Entities.Permissions
{
    public class Permission
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;

        public ICollection<Role> Roles { get; set; } = new List<Role>();

        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

    }
}
