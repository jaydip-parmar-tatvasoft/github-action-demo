using Domain.Entities.Users;

namespace Domain.Entities.Roles
{
    public class UserRole
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;
    }
}
