using GitHubActionDemo.Entity;

namespace GitHubActionDemo.Service
{
    public sealed class LoginUser(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        public record Request(string Email, string password);

        public async Task<User> Handle(Request request)
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
            return user;
        }
    }
}
