using Domain.Entities.Permissions;
using Domain.Entities.Roles;

namespace GitHubActionDemo.Entities
{
    public class RolePermission
    {
        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;

        public int PermissionId { get; set; }
        public Permission Permission { get; set; } = null!;
    }
}
