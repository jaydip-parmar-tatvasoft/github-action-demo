using Carter;

namespace GitHubActionDemo.StartUps;

public static class EndPointStartup
{
    public static IServiceCollection AddEndPoints(this IServiceCollection services)
    {
        services.AddCarter();
        services.AddEndpointsApiExplorer();
        return services;
    }

    public static WebApplication UseEndPoints(this WebApplication app)
    {
        app.MapCarter();
        app.UseHttpsRedirection();

        return app;
    }
}
