using GitHubActionDemo.Enums;
using GitHubActionDemo.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace GitHubActionDemo.Authentication
{
    public sealed class HasPermissionAttribute : AuthorizeAttribute
    {
        public HasPermissionAttribute(Permission permission) : base(policy: permission.GetEnumDescription())
        {
        }
    }
}
