using Domain.Entities.Users;
using System.Security.Claims;

namespace Infrastructure.Authentication
{
    public interface IJwtProvider
    {
        Task<string> CreateAccessTokenAsync(UserEntity userEntity);

        string CreateRefreshToken();

        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);

    }
}
