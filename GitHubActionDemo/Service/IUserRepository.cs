using GitHubActionDemo.Entity;

namespace GitHubActionDemo.Service
{
    public interface IUserRepository
    {
        Task InsertAsync(User user);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> UserNameExistsAsync(string userName);
        Task<User?> GetByEmail(string email);
    }
}
