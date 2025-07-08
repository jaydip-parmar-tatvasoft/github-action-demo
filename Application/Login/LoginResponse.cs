namespace Application.Login
{
    public record LoginResponse(string AccessToken, string RefreshToken, DateTime RefreshTokenExpiresOn);
}
