using Orion.Models;
using Orion.Models.ClientTransmissions;
using Orion.Models.ServerTransmissions.Results;
using Orion.Server.DirectCommunications;

namespace Orion.Server.Users.Repositories
{
    public interface IUserRepository
    {
        public Task<ServerResult<AuthenticationMessages>?> AuthenticateUser(Credentials credentials);
        
        public Task<User?> AddUser(string username);
        
        public Task<User?> GetUser(Guid userId);
    }
}