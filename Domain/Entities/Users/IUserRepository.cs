namespace Domain.Entities.Users
{
    public interface IUserRepository
    {
        Task CreateAsync(UserEntity member, CancellationToken cancellationToken);
        Task<List<UserEntity>> GetAll(CancellationToken cancellationToken);
        Task<UserEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<UserEntity?> GetByEmailAsync(string email, CancellationToken cancellationToken);
        Task<UserEntity?> GetByUsernameAsync(string username, CancellationToken cancellationToken);
        Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken);
        Task<bool> IsUsernameUniqueAsync(string username, CancellationToken cancellationToken);
    }
}
