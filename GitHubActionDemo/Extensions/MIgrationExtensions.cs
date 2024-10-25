using GitHubActionDemo.Database;
using GitHubActionDemo.Entity;
using Microsoft.EntityFrameworkCore;

namespace GitHubActionDemo.Extensions
{
    public static class MIgrationExtensions
    {
        public static void AddMigration(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using ApplicationDbContext context = scope.ServiceProvider.GetService<ApplicationDbContext>()!;

            context.Database.Migrate();

            //  SeedData(context);
        }
    }
}
