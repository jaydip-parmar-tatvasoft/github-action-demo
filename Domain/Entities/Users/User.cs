using Domain.Entities.Roles;

namespace Domain.Entities.Users
{
    public class User
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpireOnUtc { get; set; }

        public User() { }

        public ICollection<Role> Roles { get; set; } = new List<Role>();

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
