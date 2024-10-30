using GitHubActionDemo.Database;
using Microsoft.EntityFrameworkCore;

namespace GitHubActionDemo.Extensions
{
    public static class MigrationExtensions
    {
        public static void AddMigration(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using ApplicationDbContext context = scope.ServiceProvider.GetService<ApplicationDbContext>()!;
            //context.Database.Migrate();

            //  SeedData(context);
        }
    }
}
