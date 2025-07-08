using Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GitHubActionDemo.Options;

public class JwtBearerOptionsSetup : IConfigureNamedOptions<JwtBearerOptions>, IConfigureOptions<JwtBearerOptions>
{
    private readonly JwtOptions mJwtOptions;

    public JwtBearerOptionsSetup(IOptions<JwtOptions> jwtOptions)
    {
        mJwtOptions = jwtOptions.Value;
    }

    public void Configure(JwtBearerOptions options)
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new()
        {
            //ValidateIssuer = true,
            //ValidateAudience = true,
            //ValidateLifetime = true,
            //ValidateIssuerSigningKey = true,
            //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(mJwtOptions.Secret))

            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(mJwtOptions.Secret)),
            ValidIssuer = mJwtOptions.Issuer,
            ValidAudience = mJwtOptions.Audience,
            ClockSkew = TimeSpan.Zero
        };
    }

    public void Configure(string? name, JwtBearerOptions options)
    {
        Configure(options);
    }
}

