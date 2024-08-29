using Orion.Models;
using Orion.Models.ClientTransmissions;
using Orion.Models.ServerTransmissions;
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

        public async Task<ServerTransmission> AuthenticateUser(Credentials credentials)
        {
            var user = (await _database.UserEntity.GetAllRecords())
                        .Where(x => x.Username == credentials.Username)
                        .Select(x => new User { UserId = x.UserId, Username = x.Username })
                        .SingleOrDefault();

            user.DirectCommunicationIds 

            return (user == null)
                ? ServerResultService.NewSuccessfulResponseServerTransmission("AuthenticateUserResult", AuthenticationMessages.INVALID_CREDENTIALS)
                    .AddResponseOperationMessage("Invalid credentials entered")
                : ServerResultService.NewSuccessfulResponseServerTransmission("AuthenticateUserResult", AuthenticationMessages.VALID_CREDENTIALS, user)
                    .AddResponseAffectedUser(user.UserId)
                    .AddResponseOperationMessage("Valid credentials entered");
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