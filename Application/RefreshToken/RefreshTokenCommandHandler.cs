using Application.Abstractions.Messaging;
using Data.Repositories;
using Domain.Entities.Users;
using Domain.Shared;
using Infrastructure.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Application.RefreshToken
{
    internal sealed class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, RefreshTokenResponse>
    {
        private readonly IJwtProvider jwtProvider;
        private readonly IUserRepository userRepository;

        public RefreshTokenCommandHandler(IJwtProvider jwtProvider, IUserRepository userRepository)
        {
            this.jwtProvider = jwtProvider;
            this.userRepository = userRepository;
        }


        public async Task<Result<RefreshTokenResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var climPrincipal = jwtProvider.GetPrincipalFromExpiredToken(request.AccessToken);
            var userId = climPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out Guid parseUserId))
            {
                return Error.AccessUnAuthorized("UnAuthorized", $"RefreshToken is expire or invalid");
            }

            var user = await userRepository.GetByIdAsync(parseUserId, cancellationToken);

            if (user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpireOnUtc < DateTime.UtcNow)
            {
                return Error.AccessUnAuthorized("UnAuthorized", $"RefreshToken is expire or invalid");
            }

            var token = await jwtProvider.CreateAccessTokenAsync(user);
            var refreshToken = jwtProvider.CreateRefreshToken();
            //user.RefreshTokenExpireOnUtc = DateTime.UtcNow.AddDays(1);
            
            var response = new RefreshTokenResponse(token, refreshToken, user.RefreshTokenExpireOnUtc);
            return response;
        }
    }
}
