using GitHubActionDemo.Entities;

namespace GitHubActionDemo.Service
{
    public interface IUserRepository
    {
        Task InsertAsync(User user);
        Task UpdateAsync(User user);

        Task<bool> EmailExistsAsync(string email);
        Task<bool> UserNameExistsAsync(string userName);
        Task<User?> GetByEmail(string email);
        Task<User?> GetById(string userId);
    }
}
