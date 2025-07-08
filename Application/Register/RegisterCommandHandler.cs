using Application.Abstractions.Messaging;
using Domain;
using Domain.Entities.Roles;
using Domain.Entities.Users;
using Domain.Shared;
using Infrastructure.Authentication;

namespace Application.Register
{
    internal sealed class RegisterCommandHandler : ICommandHandler<RegisterCommand, Guid>
    {
        private readonly IUserRepository userRepository;
        private readonly IPasswordHasher passwordHasher;
        private readonly IRoleRepository roleRepository;
        private readonly IJwtProvider jwtProvider;
        private readonly IUnitOfWork unitOfWork;

        public RegisterCommandHandler(
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IRoleRepository roleRepository,
            IJwtProvider jwtProvider,
            IUnitOfWork unitOfWork
            )
        {
            this.userRepository = userRepository;
            this.passwordHasher = passwordHasher;
            this.roleRepository = roleRepository;
            this.jwtProvider = jwtProvider;
            this.unitOfWork = unitOfWork;
        }


        public async Task<Result<Guid>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (!await userRepository.IsUsernameUniqueAsync(request.UserName, cancellationToken))
            {
                return Error.Validation("Username", "Username already exists");
            }

            if (!await userRepository.IsEmailUniqueAsync(request.Email, cancellationToken))
            {
                return Error.Validation("Email", "Email already exists.");
            }

            var passwordHash = passwordHasher.Hash(request.Password);

            Role? DefaultRole = await roleRepository.GetByName("User", cancellationToken);
            List<Role> Roles = (DefaultRole is not null) ? new() { DefaultRole } : new();

            var user = UserEntity.Create(request.UserName, request.Email, passwordHash, Roles);

            await userRepository.CreateAsync(user, cancellationToken);
            await unitOfWork.SaveChangesAsync();
            return user.Id;
        }
    }
}
