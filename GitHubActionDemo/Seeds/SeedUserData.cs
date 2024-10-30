using GitHubActionDemo.Entities;
using GitHubActionDemo.Service;

namespace GitHubActionDemo.Seeds
{
    public class SeedUserData
    {
        private readonly IPasswordHasher passwordHasher;
        private readonly ITokenProvider tokenProvider;

        public SeedUserData(IPasswordHasher passwordHasher, ITokenProvider tokenProvider)
        {
            this.passwordHasher = passwordHasher;
            this.tokenProvider = tokenProvider;
        }

        public User CreateUser(string email, string password)
        {
            return new User
            {
                Email = email,
                UserId = new Guid("6f47a3a1-4c15-4f7b-a62f-df9e36e2543a"),
                PasswordHash = passwordHasher.Hash(password),
                RefreshToken = tokenProvider.CreateRefreshToken(),
                UserName = email,
                RefreshTokenExpireOnUtc = DateTime.UtcNow.AddDays(1),
                Roles = new[] { Role.Registered },
            };
        }
    }
}
