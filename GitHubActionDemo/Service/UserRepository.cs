using GitHubActionDemo.Database;
using Microsoft.EntityFrameworkCore;
using static Dapper.SqlMapper;
using System.Collections.Generic;
using GitHubActionDemo.Entities;

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

        public async Task UpdateAsync(User user)
        {
            dbContext.Set<User>().Update(user);
            await dbContext.SaveChangesAsync();
        }
    }
}
