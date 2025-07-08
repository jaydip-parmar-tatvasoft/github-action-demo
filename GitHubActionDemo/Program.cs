using Data.Startup;
using GitHubActionDemo.Options;
using GitHubActionDemo.StartUps;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddApplicationLogging();

builder.Services.ConfigureOptions<DatabaseOptionsSetup>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

