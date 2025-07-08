using Data.Options;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Data.Startup;

public static class DataAccessDatabaseStartup
{
    public static void Implement(IServiceCollection services)
    {
        ImplementDefault(services);
    }

    private static void ImplementDefault(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            var databaseOptions = sp.GetService<IOptions<DatabaseOptions>>()!.Value;
            options.UseNpgsql(databaseOptions.ConnectionString);
        });
    }
}
