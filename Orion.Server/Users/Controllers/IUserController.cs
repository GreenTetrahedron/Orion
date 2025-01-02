using Orion.Models.ServerTransmissions;
using Orion.Models.UserModels;

namespace Orion.Server.Users.Controllers
{
    public interface IUserController
    {
        public Task<ServerTransmission> AuthenticateUser(Credentials credentials);
    }
}
