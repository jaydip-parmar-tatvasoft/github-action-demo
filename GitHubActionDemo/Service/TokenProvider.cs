using GitHubActionDemo.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GitHubActionDemo.Service
{
    public sealed class TokenProvider : ITokenProvider
    {
        private readonly IConfiguration configuration;
        private readonly IPermissionService permissionService;

        public TokenProvider(IConfiguration configuration, IPermissionService permissionService)
        {
            this.configuration = configuration;
            this.permissionService = permissionService;
        }

        public async Task<string> CreateAccessTokenAsync(User user)
        {
            var claims = new List<Claim> {
                new(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(ClaimTypes.Role, Role.Registered.Name),

            };

            var permissions = await permissionService.GetPermissionAsync(user.UserId);

            foreach (var item in permissions)
            {
                claims.Add(new Claim("permission", item));
            }

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                null,
                DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("Jwt:ExpirationInMinutes")),
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
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!)),
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
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
