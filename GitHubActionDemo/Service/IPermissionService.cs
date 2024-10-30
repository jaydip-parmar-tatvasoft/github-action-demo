using GitHubActionDemo.Database;
using GitHubActionDemo.Entities;
using Microsoft.EntityFrameworkCore;

namespace GitHubActionDemo.Service
{
    public interface IPermissionService
    {
        Task<HashSet<string>> GetPermissionAsync(Guid userId);
    }

    public sealed class PermissionService : IPermissionService
    {
        private readonly ApplicationDbContext dbContext;

        public PermissionService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<HashSet<string>> GetPermissionAsync(Guid userId)
        {
            var roles = await dbContext.Set<User>()
                .Include(x => x.Roles)
                .ThenInclude(x => x.Permissions)
                .Where(x => x.UserId == userId)
                .Select(x => x.Roles).ToArrayAsync();

            return roles
                .SelectMany(role => role)
                .SelectMany(x => x.Permissions)
                .Select(s => s.Name)
                .ToHashSet();

        }
    }
}
