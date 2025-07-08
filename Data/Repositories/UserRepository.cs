using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using GitHubActionDemo.Entities;
using Domain.Entities.Users;
using Data.Exceptions;

namespace Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext dbContext;
        public UserRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CreateAsync(UserEntity member, CancellationToken cancellationToken)
        {
            UserEntity? ExistingItem = await GetByIdAsync(member.Id, cancellationToken);
            if (ExistingItem is not null)
                throw new DuplicateDataException("User Id", member.Id.ToString());

            User MappedItem = member.Map();
            await dbContext.Users.AddAsync(MappedItem);
        }

        public async Task<List<UserEntity>> GetAll(CancellationToken cancellationToken)
        {
            List<User> user = await dbContext.Users.Include(r => r.Roles).ToListAsync(cancellationToken);
            return user.Select(s => s.Map()).ToList();
        }

        public async Task<UserEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            var user = await dbContext.Users.Include(r => r.Roles).FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

            if (user is null)
                return default;

            return user.Map();
        }

        public async Task<UserEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            User? user = await dbContext.Users.Include(r => r.Roles).FirstOrDefaultAsync(u => u.UserId == id, cancellationToken);

            if (user is null)
                return default;

            return user.Map();

        }

        public async Task<UserEntity?> GetByUsernameAsync(string username, CancellationToken cancellationToken)
        {
            var user = await dbContext.Users.Include(r => r.Roles).FirstOrDefaultAsync(u => u.UserName == username, cancellationToken);

            if (user is null)
                return default;

            return user.Map();
        }

        public async Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken)
        {
            return !await dbContext.Users.AnyAsync(u => u.Email == email, cancellationToken);
        }

        public async Task<bool> IsUsernameUniqueAsync(string username, CancellationToken cancellationToken)
        {
            return !await dbContext.Users.AnyAsync(u => u.UserName == username, cancellationToken);
        }

        //public async Task InsertAsync(User user)
        //{
        //    dbContext.users.Add(user);
        //    await dbContext.SaveChangesAsync();
        //}
        //public async Task<bool> EmailExistsAsync(string email)
        //{
        //    return await dbContext.users.AnyAsync(u => u.Email == email);
        //}
        //public async Task<bool> UserNameExistsAsync(string userName)
        //{
        //    return await dbContext.users.AnyAsync(u => u.UserName == userName);
        //}

        //public async Task<User?> GetByEmail(string email)
        //{
        //    return await dbContext.users.SingleOrDefaultAsync(u => u.Email == email);
        //}

        //public async Task<User?> GetById(string userId)
        //{
        //    return await dbContext.users.FindAsync(new Guid(userId));
        //}

        //public async Task UpdateAsync(User user)
        //{
        //    dbContext.Set<User>().Update(user);
        //    await dbContext.SaveChangesAsync();
        //}
    }
}
