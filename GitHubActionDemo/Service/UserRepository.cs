using GitHubActionDemo.Database;
using GitHubActionDemo.Entity;
using Microsoft.EntityFrameworkCore;

namespace GitHubActionDemo.Service
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext dbContext;
        public UserRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task InsertAsync(User user)
        {
            dbContext.users.Add(user);
            await dbContext.SaveChangesAsync();
        }
        public async Task<bool> EmailExistsAsync(string email)
        {
            return await dbContext.users.AnyAsync(u => u.Email == email);
        }
        public async Task<bool> UserNameExistsAsync(string userName)
        {
            return await dbContext.users.AnyAsync(u => u.UserName == userName);
        }

        public async Task<User?> GetByEmail(string email)
        {
            return await dbContext.users.SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetById(string userId)
        {
            return await dbContext.users.FindAsync(new Guid(userId));
        }
    }
}
