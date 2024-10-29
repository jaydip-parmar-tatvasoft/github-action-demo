using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace GitHubActionDemo.Service
{
    public sealed class RefreshTokenRequest(TokenProvider tokenProvider, IUserRepository userRepository)
    {
        public record Request(string AccessToken, string RefreshToken);

        public async Task<TokenResponse?> Handle(Request request)
        {
            var climPrincipal = tokenProvider.GetPrincipalFromExpiredToken(request.AccessToken);
            var userId = climPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                throw new SecurityTokenException("Token is invalid");
            }
            
            var user = await userRepository.GetById(userId);

            if (user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpireOnUtc < DateTime.UtcNow)
            {
                throw new SecurityTokenException("RefreshToken is expire or invalid");
            }

            var token = tokenProvider.CreateAccessToken(user);
            var refreshToken = tokenProvider.CreateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpireOnUtc = DateTime.UtcNow.AddDays(1);
            await userRepository.UpdateAsync(user);
            return new TokenResponse(token, refreshToken, user.RefreshTokenExpireOnUtc);
        }
    }
}
