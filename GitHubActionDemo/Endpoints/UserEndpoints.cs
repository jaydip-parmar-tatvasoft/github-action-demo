using Application.Login;
using Application.RefreshToken;
using Application.Register;
using Carter;
using Domain.Entities.Users;
using GitHubActionDemo.Extensions;
using Infrastructure.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GitHubActionDemo.Endpoints
{
    public class UserEndpoints : CarterModule
    {
        public UserEndpoints()
        // :base("/users")
        {
        }

        public override void AddRoutes(IEndpointRouteBuilder builder)
        {
            builder.MapPost("/register", RegisterUser).AllowAnonymous();

            builder.MapPost("/login", LoginUser).AllowAnonymous();

            builder.MapGet("/user/me", GetUserDetails);

            builder.MapPost("/get-token", GetToken).AllowAnonymous();

        }

        public static async Task<IResult> RegisterUser(ISender sender, RegisterRequest request, CancellationToken cancellationToken)
        {
            var command = new RegisterCommand(request.Email, request.UserName, request.Password);

            var result = await sender.Send(command, cancellationToken);

            return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
        }

        public static async Task<IResult> LoginUser(ISender sender, [FromBody] LoginRequest request, CancellationToken cancellationToken)
        {
            var command = new LoginCommand(request.Email, request.Password);

            var result = await sender.Send(command, cancellationToken);

            if (!result.IsSuccess)
                return result.ToProblemDetails();

            return Results.Ok(result.Value);
        }

        //[HasPermission(Domain.Enums.Permission.WriteMember)]
        //[Authorize(Policy = "AdminPolicy")]
        public static async Task<IResult> GetUserDetails(HttpContext httpContext,ISender sender ,CancellationToken cancellationToken)
        {
            var userId = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null || !Guid.TryParse(userId, out Guid parseUserId))
            {
                return Results.Unauthorized();
            }

            var command = new LoginCommand(request.Email, request.Password);


            var userDetails = await userRepository.GetByIdAsync(parseUserId, cancellationToken);
            return Results.Ok(userDetails);
        }

        public async Task<IResult> GetToken(ISender sender, RefreshTokenRequest request, CancellationToken cancellationToken)
        {
            var command = new RefreshTokenCommand(request.AccessToken, request.RefreshToken);

            var result = await sender.Send(command);

            if (!result.IsSuccess)
                return result.ToProblemDetails();

            return Results.Ok(result.Value);
        }
    }
}
