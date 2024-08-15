using Orion.Models.ServerTransmissions.Results;
using Orion.Server.DataLayer;
using Orion.Server.Messages;
using Orion.Server.ServerResults;
using Orion.Server.Users;
using System.Linq;

namespace Orion.Server.Users.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDataLayer _dataLayer;

        public UserRepository(IDataLayer dataLayer)
        {
            _dataLayer = dataLayer;
        }

        public async Task<ServerResult?> AuthenticateUser(string username)
        {
            var users = await _dataLayer.GetAllUsers();

            var user = users?
                .Select(x => x)
                .Where(x => x.Username == username)
                .SingleOrDefault();

            return (user == null)
                ? ServerResultService.NewServerResult(OperationMessages.INVALID_CREDENTIALS)
                : ServerResultService.NewServerResult(OperationMessages.VALID_CREDENTIALS, user.UserId, user);
        }

        public async Task<User?> AddUser(string username)
        {
            var user = new User() { 
                UserId = Guid.NewGuid(),
                Username = username
            };

            await _dataLayer.AddUser(user);

            return user;
        }

        public async Task<User?> GetUser(Guid userId)
        {
            return await _dataLayer.GetUser(userId);
        }
    }
}