using Orion.Models.ServerTransmissions.Results;
using Orion.Server.Users;

namespace Orion.Server.Users.Repositories
{
    public interface IUserRepository
    {
        public Task<ServerResult?> AuthenticateUser(string username);
        public Task<User?> AddUser(string username);
        public Task<User?> GetUser(Guid userId);
    }
}