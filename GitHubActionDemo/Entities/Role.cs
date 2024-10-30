using GitHubActionDemo.Primitive;

namespace GitHubActionDemo.Entities
{
    public sealed class Role : Enumeration<Role>
    {
        public static readonly Role Registered = new(1, "Registered");

        public Role()
        {
        }

        private Role(int id, string value) : base(id, value)
        {

        }

        public ICollection<Permission> Permissions { get; set; } = [];
        public ICollection<User> Users { get; set; } = [];
    }
}
