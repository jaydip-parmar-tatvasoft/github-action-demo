using Data;

namespace GitHubActionDemo.Extensions
{
    public static class MigrationExtensions
    {
        public static void AddMigration(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using ApplicationDbContext context = scope.ServiceProvider.GetService<ApplicationDbContext>()!;
        }
    }
}
