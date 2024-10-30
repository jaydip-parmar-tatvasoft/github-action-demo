using GitHubActionDemo.Entities;
using GitHubActionDemo.Enums;

namespace GitHubActionDemo.Seeds
{
    public static class SeedRolePermission
    {
        public static RolePermission Create(Role role, Enums.Permission permission)
        {
            return new RolePermission
            {
                RoleId = role.Id,
                PermissionId = (int)permission,
            };
        }
    }
}
