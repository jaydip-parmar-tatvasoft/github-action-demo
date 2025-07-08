using Application.Abstractions.Messaging;

namespace Application.RefreshToken
{
    public record RefreshTokenCommand(string AccessToken, string RefreshToken) : ICommand<RefreshTokenResponse>;
}
