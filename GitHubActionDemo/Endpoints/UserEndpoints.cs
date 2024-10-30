using GitHubActionDemo.Authentication;
using GitHubActionDemo.Enums;
using GitHubActionDemo.Service;
using Microsoft.AspNetCore.Authorization;
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

            builder.MapGet("/user/me",
            //[HasPermission(Permission.ViewUser)]
             [Authorize(Policy = "AdminPolicy")]
            async (HttpContext httpContext, IUserRepository userRepository) =>
            {
                var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
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
