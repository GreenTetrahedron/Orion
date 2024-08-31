using Orion.Models;
using Orion.Models.ClientTransmissions;
using Orion.Models.ServerTransmissions;
using Orion.Models.ServerTransmissions.Results;
using Orion.Models.UserModels;
using Orion.Server.DirectCommunications;

namespace Orion.Server.Users.Repositories
{
    public interface IUserRepository
    {
        public Task<ServerTransmission> AuthenticateUser(Credentials credentials);
        
        public Task<UserDTO?> AddUser(string username);
        
        public Task<UserDTO?> GetUser(Guid userId);

        public Task<ServerTransmission> GetUserProfileByUsername(string username);
    }
}