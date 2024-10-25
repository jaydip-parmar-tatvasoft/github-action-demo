using GitHubActionDemo.Service;
using static GitHubActionDemo.Service.RegisterUser;

namespace GitHubActionDemo.Endpoints
{
    public static class UserEndpoints
    {
        public static IEndpointRouteBuilder Map(IEndpointRouteBuilder builder)
        {
            builder.MapPost("/register", async (RegisterUser.Request request, RegisterUser registerUser) =>
                await registerUser.Handle(request));

            builder.MapPost("/login", async (LoginUser.Request request, LoginUser loginUser) =>
               await loginUser.Handle(request));

            return builder;
        }
    }
}
