using Orion.Models.ServerTransmissions.Results;
using Orion.Server.DirectCommunications;

namespace Orion.Server.Users.Repositories
{
    public interface IUserRepository
    {
        public Task<ServerResult?> AuthenticateUser(string username);
        
        public Task<User?> AddUser(string username);
        
        public Task<User?> GetUser(Guid userId);

        public Task<ServerResult?> AddDirectCommunication(Guid senderId, Guid recipientId);
    }
}