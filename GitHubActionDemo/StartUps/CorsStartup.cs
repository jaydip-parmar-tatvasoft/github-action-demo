using Carter;

namespace GitHubActionDemo.StartUps;

public static class CorsStartup
{
    public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
    {
        services.AddCors(options =>
         {
             options.AddPolicy("AllowLocalhost", builder =>
             {
                 builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
             });
         });
        return services;
    }

    public static WebApplication UseCorsPolicy(this WebApplication app)
    {
        app.UseCors("AllowLocalhost");
        return app;
    }

}
