using Orion.Models.ClientTransmissions;
using Orion.Models.ServerTransmissions;
using Orion.Models.ServerTransmissions.Results;
using Orion.Models.UserModels;

namespace Orion.Server.Users.Controllers
{
    public interface IUserController
    {
        public Task<ServerTransmission> AuthenticateUser(Credentials credentials);
        public Task<ServerTransmission> GetUserByUsername(string username);
    }
}
