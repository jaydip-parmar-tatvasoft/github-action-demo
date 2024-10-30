
using Dapper;
using GitHubActionDemo.Database;
using GitHubActionDemo.Entities;
using GitHubActionDemo.Service;
using Microsoft.EntityFrameworkCore;

namespace GitHubActionDemo
{
    public class GuidService : IGuidService
    {
        private readonly Guid _guid;
        private readonly NpgsqlConnectionFactory _connectionFactory;
        private readonly ApplicationDbContext dbContext;

        public GuidService(NpgsqlConnectionFactory npgsqlConnectionFactory,
            ApplicationDbContext dbContext)
        {
            _guid = Guid.NewGuid();
            this._connectionFactory = npgsqlConnectionFactory;
            this.dbContext = dbContext;
        }

        public string GetGuid()
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                var user = connection.Query<User>(
                   $"""
                   SELECT user_id AS {nameof(User.UserId)},
                   user_name AS {nameof(User.UserName)} 
                   FROM users where user_id = @UserId;
                   """
                  , new
                  {
                      UserId = new Guid("795f7cf5-3fe8-46f5-be58-f0b11d867e1a")
                  });
            }
            var users = dbContext.users.ToList();
            return _guid.ToString();
        }
    }
}
