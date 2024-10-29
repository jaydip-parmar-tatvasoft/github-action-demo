using GitHubActionDemo;
using GitHubActionDemo.Database;
using GitHubActionDemo.Endpoints;
using GitHubActionDemo.Extensions;
using GitHubActionDemo.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithAuth();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

builder.Services.AddSingleton<NpgsqlConnectionFactory>(provider =>
{
    return new NpgsqlConnectionFactory(connectionString);
});

builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseNpgsql(connectionString);
});

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
    {
        option.RequireHttpsMetadata = false;
        option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]!)),
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddScoped<IGuidService, GuidService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<RegisterUser>();
builder.Services.AddScoped<LoginUser>();
builder.Services.AddScoped<RefreshTokenRequest>();
builder.Services.AddSingleton<TokenProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.AddMigration();
}

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();


app.MapGet("/get-guid", (IGuidService guidService) =>
{
    return guidService.GetGuid();
})
.WithName("GETGUID")
.WithOpenApi();

UserEndpoints.Map(app);

app.UseAuthentication();

app.UseAuthorization();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
