using GitHubActionDemo.Entity;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GitHubActionDemo.Service
{
    public sealed class TokenProvider(IConfiguration configuration)
    {
        public string CreateAccessToken(User user)
        {
            string secertKey = configuration["Jwt:Secret"]!;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secertKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity
                ([
                   new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                   new Claim(JwtRegisteredClaimNames.Email, user.Email),
                 ]),
                Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("Jwt:ExpirationInMinutes")),
                SigningCredentials = credentials,
                Issuer = configuration["Jwt:Issuer"],
                Audience = configuration["Jwt:Audience"]
            };
            var handeler = new JsonWebTokenHandler();
            var token = handeler.CreateToken(tokenDescriptor);
            return token;
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
