using Domain.Entities.Roles;
using Domain.Shared;

namespace Domain.Entities.Users
{
    public sealed class UserEntity : AggregateRoot
    {
        private UserEntity(Guid id, string userName, string email, string passwordHash, List<Role> roles) :
            base(id)
        {
            UserName = userName;
            Email = email;
            PasswordHash = passwordHash;
            Roles = roles;
        }

        public string UserName { get; private set; }

        public string Email { get; private set; }

        public string PasswordHash { get; private set; }

        public string RefreshToken { get; private set; }

        public DateTime RefreshTokenExpireOnUtc { get; private set; }

        public ICollection<Role> Roles { get; set; } = new List<Role>();

        public static UserEntity Create(Guid id, string userName, string email, string passwordHash, List<Role> roles)
        {
            return new UserEntity(id, userName, email, passwordHash, roles);
        }

        public static UserEntity Create(string userName, string email, string passwordHash, List<Role> roles)
        {
            return new UserEntity(Guid.NewGuid(), userName, email, passwordHash, roles);
        }

    }
}
