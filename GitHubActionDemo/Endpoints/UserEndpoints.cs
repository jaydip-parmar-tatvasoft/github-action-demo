using GitHubActionDemo.Entity;
using GitHubActionDemo.Service;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
using System.Text;

namespace GitHubActionDemo.Endpoints
{
    public static class UserEndpoints
    {
        public static IEndpointRouteBuilder Map(IEndpointRouteBuilder builder)
        {
            builder.MapPost("/register", async (RegisterUser.Request request, RegisterUser registerUser) =>
                await registerUser.Handle(request))
                .AllowAnonymous();

            builder.MapPost("/login", async (LoginUser.Request request, LoginUser loginUser) =>
               await loginUser.Handle(request))
                .AllowAnonymous();

            builder.MapGet("/user/me", async (HttpContext httpContext, IUserRepository userRepository) =>
            {
                var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var builder = new StringBuilder();

                foreach (var item in httpContext.User.Claims)
                {
                    builder.AppendLine($"{item.Type}- {item.Value}");
                }
                var x = builder.ToString(); 

                if (userId == null)
                {
                    return Results.Unauthorized();
                }
                return Results.Ok(await userRepository.GetById(userId));
            });

            builder.MapPost("/get-token", async (RefreshTokenRequest.Request request, RefreshTokenRequest refreshTokenRequest) =>
               await refreshTokenRequest.Handle(request)).AllowAnonymous();

            return builder;
        }
    }
}
