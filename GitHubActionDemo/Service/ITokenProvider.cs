using GitHubActionDemo.Entities;
using System.Security.Claims;

namespace GitHubActionDemo.Service
{
    public interface ITokenProvider
    {
        Task<string> CreateAccessTokenAsync(User user);

        string CreateRefreshToken();

        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
