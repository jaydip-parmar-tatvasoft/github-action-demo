using Domain.Entities.Permissions;
using Domain.Entities.Users;
using GitHubActionDemo.Entities;

namespace Domain.Entities.Roles
{
    public sealed class Role
    {
        public Role() { }

        public int Id { get; private set; }
        public string Name { get; private set; }


        public Role(int id, string name) 
        {
            Id = id;
            Name = name;
        }

        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        public ICollection<Permission> Permissions { get; set; } = new List<Permission>();

        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

    }
}
