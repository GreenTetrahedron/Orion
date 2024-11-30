using Orion.Models.ServerTransmissions;
using Orion.Models.ServerTransmissions.Results;
using Orion.Models.UserModels;

namespace Orion.Server.Users.Repositories
{
    public interface IUserRepository
    {
        public Task<ServerTransmission> AuthenticateUser(Credentials credentials);

        public Task<UserDTO?> AddUser(Credentials credentials);

        public Task<UserDTO?> GetUser(Guid userId);

        public Task<ServerTransmission> GetUserProfileByUsername(string username);

        public Task<Roles?> GetRoleByUserId(Guid userId);

        public Task<ServerResult> AuthenticateSuperadmin(Credentials credentials);
    }
}