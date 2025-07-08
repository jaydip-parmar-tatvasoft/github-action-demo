using Application.Abstractions.Messaging;
using Domain.Entities.Users;
using Domain.Errors;
using Domain.Shared;
using Infrastructure.Authentication;

namespace Application.Login
{
    internal sealed class LoginCommandHandler(IUserRepository userRepository, IJwtProvider jwtProvider, IPasswordHasher passwordHasher)
        : ICommandHandler<LoginCommand, LoginResponse>
    {
        private readonly IUserRepository userRepository = userRepository;
        private readonly IJwtProvider jwtProvider = jwtProvider;
        private readonly IPasswordHasher passwordHasher = passwordHasher;

        public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByEmailAsync(request.Email, cancellationToken);

            if (user is null)
            {
                return DomainErrors.User.UserNotFound(request.Email);
            }

            bool verified = passwordHasher.Verify(request.Password, user.PasswordHash);

            if (!verified)
            {
                return DomainErrors.User.InvalidCredentials;
            }
            var token = await jwtProvider.CreateAccessTokenAsync(user);
            var refreshToken = jwtProvider.CreateRefreshToken();

            //user.RefreshToken = refreshToken;
            //user.RefreshTokenExpireOnUtc = DateTime.UtcNow.AddDays(1);
            //await userRepository.UpdateAsync(user);
            return new LoginResponse(token, refreshToken, user.RefreshTokenExpireOnUtc);
        }
    }
}
