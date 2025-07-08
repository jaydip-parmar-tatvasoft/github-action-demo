using Domain.Entities.Roles;
using Domain.Entities.Users;
using Domain.Enums;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Authentication
{
    public sealed class JwtProvider : IJwtProvider
    {
        private readonly JwtOptions options;
        private readonly IPermissionService permissionService;

        public JwtProvider(IOptions<JwtOptions> options, IPermissionService permissionService)
        {
            this.options = options.Value;
            this.permissionService = permissionService;
        }

        public async Task<string> CreateAccessTokenAsync(UserEntity user)
        {
            var claims = new List<Claim> {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
                //new(ClaimTypes.Role, RoleType.),
                //new(ClaimTypes.Role, ),
            };

            var permissions = await permissionService.GetPermissionsAsync(user.Id);

            foreach (var item in permissions)
            {
                claims.Add(new Claim("permission", item));
            }

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(options.Secret)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(options.Issuer, options.Audience,
                claims,
                null,
                DateTime.UtcNow.AddSeconds(options.ExpirationInSeconds),
                signingCredentials);

            string tokenValue = new JwtSecurityTokenHandler()
                .WriteToken(token);

            return tokenValue;
        }

        public string CreateRefreshToken()
        {
            var randomNumber = new byte[32];

            using (var rang = RandomNumberGenerator.Create())
            {
                rang.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Secret)),
                ValidIssuer = options.Issuer,
                ValidAudience = options.Audience,
                ValidateLifetime = false,
            };
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
            var jwtSecurityToken = securityToken as System.IdentityModel.Tokens.Jwt.JwtSecurityToken;

            if (jwtSecurityToken is null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("token is invalid");
            }
            return principal;
        }
    }
}
