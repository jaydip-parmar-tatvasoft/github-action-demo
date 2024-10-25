using GitHubActionDemo.Entity;
using GitHubActionDemo.Service;
using Microsoft.IdentityModel.JsonWebTokens;

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
                var userId = httpContext.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
                  ?? httpContext.User.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;


                var claims = httpContext.User.Claims.Select(c => new { c.Type, c.Value }).ToList();

                if (userId == null)
                {
                    throw new Exception("Please login");
                }
                return await userRepository.GetById(userId);

            }).RequireAuthorization();

            return builder;
        }
    }
}
