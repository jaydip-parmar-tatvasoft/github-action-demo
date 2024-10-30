using GitHubActionDemo.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace GitHubActionDemo.Authentication
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!Guid.TryParse(userId, out var parsedUserId))
            {
                return Task.CompletedTask;
            }
            var permissions = context.User.Claims
                .Where(x => x.Type == "permission")
                .Select(s => s.Value).ToHashSet();

            if (permissions.Contains(requirement.Permission))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
