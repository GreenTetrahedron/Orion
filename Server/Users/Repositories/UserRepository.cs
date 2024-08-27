using Orion.Models;
using Orion.Models.ClientTransmissions;
using Orion.Models.ServerTransmissions.Results;
using Orion.Server.DataLayer;
using Orion.Server.DataLayer.Entities;
using Orion.Server.DirectCommunications;
using Orion.Server.ServerResults;

namespace Orion.Server.Users.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly Database _database;

        public UserRepository(Database database)
        {
            _database = database;
        }

        public async Task<ServerResult<AuthenticationMessages>?> AuthenticateUser(Credentials credentials)
        {
            var user = (await _database.UserEntity.GetAllRecords())
                        .Select(x => x)
                        .Where(x => x.Username == credentials.Username)
                        .SingleOrDefault();

            return (user == null)
                ? ServerResultService.NewSuccessfulServerResult(AuthenticationMessages.INVALID_CREDENTIALS, "Invalid credentials given")
                : ServerResultService.NewSuccessfulServerResult(AuthenticationMessages.VALID_CREDENTIALS, user.UserId, "Valid credentials", user);
        }

        public async Task<User?> AddUser(string username)
        {
            var user = new User()
            {
                UserId = Guid.NewGuid(),
                Username = username
            };

            await _database.UserEntity.AddRecord(user.UserId, user);

            return user;
        }

        public async Task<User?> GetUser(Guid userId)
        {
            return await _database.UserEntity.GetRecordById(userId);
        }
    }
}