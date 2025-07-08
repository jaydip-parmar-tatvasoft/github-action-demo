using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.RefreshToken
{
    public record RefreshTokenResponse(string AccessToken, string RefreshToken, DateTime RefreshTokenExpiresOn);
}
