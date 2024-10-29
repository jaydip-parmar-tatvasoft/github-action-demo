using GitHubActionDemo.Entity;

namespace GitHubActionDemo.Service
{
    public sealed class LoginUser(IUserRepository userRepository, IPasswordHasher passwordHasher, TokenProvider tokenProvider)
    {
        public record Request(string Email, string password);

        public async Task<TokenResponse> Handle(Request request)
        {
            User? user = await userRepository.GetByEmail(request.Email);

            if (user is null)
            {
                throw new Exception("User not found.");
            }

            bool verified = passwordHasher.Verify(request.password, user.PasswordHash);

            if (!verified)
            {
                throw new Exception("The password is incorrect.");
            }
            var token = tokenProvider.CreateAccessToken(user);
            var refreshToken = tokenProvider.CreateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpireOnUtc = DateTime.UtcNow.AddDays(1);
            await userRepository.UpdateAsync(user);
            return new TokenResponse(token, refreshToken, user.RefreshTokenExpireOnUtc);
        }
    }

    public record TokenResponse(string AccessToken, string RefreshToken, DateTime RefreshTokenExpiresOn);
}
