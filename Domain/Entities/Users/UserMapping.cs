namespace Domain.Entities.Users
{
    public static class UserMapping
    {
        public static User Map(this UserEntity entity)
        {
            User NewItem = new User()
            {
                UserId = entity.Id,
                UserName = entity.UserName,
                Email = entity.Email,
                PasswordHash = entity.PasswordHash,
                Roles = entity.Roles.ToList()
            };
            return NewItem;
        }

        public static UserEntity Map(this User user)
        {
            return UserEntity.Create(user.UserId, user.UserName, user.Email, user.PasswordHash, user.Roles.ToList());
        }
    }
}
