using Data.Startup;
using GitHubActionDemo.Options;
using GitHubActionDemo.StartUps;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddApplicationLogging();

builder.Services.ConfigureOptions<DatabaseOptionsSetup>();

builder.Services
    .AddProblemDetails()
    .AddDataAccessEntityFramework(builder.Configuration)
    .AddMediator()
    .AddSecurity()
    .AddEndPoints()
    .AddCorsPolicy()
    .AddSwaggerWithAuth();


var app = builder.Build();

app
    .UseEndPoints()
    .UseSwaggerWithAuth()
    .UseSecurity()
    .UseCorsPolicy()
    .Run();

