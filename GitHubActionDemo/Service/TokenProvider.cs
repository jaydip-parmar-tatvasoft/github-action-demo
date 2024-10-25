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
        public string Create(User user)
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
    }
}
