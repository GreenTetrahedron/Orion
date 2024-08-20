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

        public async Task<ServerResult?> AuthenticateUser(string username)
        {
            var user = (await _database.UserEntity.GetAllRecords())
                        .Select(x => x)
                        .Where(x => x.Username == username)
                        .SingleOrDefault();

            return (user == null)
                ? ServerResultService.NewServerResult(OperationMessages.INVALID_CREDENTIALS)
                : ServerResultService.NewServerResult(OperationMessages.VALID_CREDENTIALS, user.UserId, user);
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

        public async Task<ServerResult?> AddDirectCommunication(Guid senderId, Guid recipientId)
        {
            var directCommunication = new DirectCommunication()
            {
                DirectCommunicationId = Guid.NewGuid(),
                UserIds = new Tuple<Guid, Guid>(senderId, recipientId)
            };

            bool wasSuccessful = await _database.DirectCommunicationEntity.AddRecord(directCommunication.DirectCommunicationId, directCommunication);

            return wasSuccessful
                ? ServerResultService.NewServerResult(
                    operationMessage: OperationMessages.DIRECT_COMMUNICATION_CREATION_SUCCEEDED,
                    affectedUsers: [senderId, recipientId],
                    data: directCommunication)
                : ServerResultService.NewServerResult(OperationMessages.DIRECT_COMMUNICATION_CREATION_FAILED, [senderId, recipientId]);
        }
    }
}