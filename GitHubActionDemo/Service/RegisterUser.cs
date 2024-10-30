using GitHubActionDemo.Entities;

namespace GitHubActionDemo.Service
{
    public sealed class RegisterUser(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        public record Request(string Email, string UserName, string Password);

        public async Task<User> Handle(Request request)
        {
            if (await userRepository.UserNameExistsAsync(request.UserName))
            {
                throw new Exception("User name already exists.");
            }

            if (await userRepository.EmailExistsAsync(request.Email))
            {
                throw new Exception("Email already exists.");
            }

            var user = new User
            {
                Email = request.Email,
                UserName = request.UserName,
                PasswordHash = passwordHasher.Hash(request.Password)
            };
            await userRepository.InsertAsync(user);
            return user;
        }
    }
}
