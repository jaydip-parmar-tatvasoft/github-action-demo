namespace GitHubActionDemo.Entities
{
    public class User
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpireOnUtc { get; set; }
        public ICollection<Role> Roles { get; set; } = [];
    }
}
