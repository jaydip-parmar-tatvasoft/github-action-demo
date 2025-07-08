using Data.Repositories;
using Domain.Entities.Roles;
using Domain.Entities.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Data.Startup;

/// <summary>
/// Initializes the Data Access Layer using Entity Framework
/// </summary>
public static class DataAccessRepositoryStartup
{

    public static void Register(IServiceCollection services)
    {

        RegisterRepositories(services);
    }

    private static void RegisterRepositories(IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
    }
}
