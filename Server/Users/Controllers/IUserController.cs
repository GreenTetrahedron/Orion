using Orion.Models.ClientTransmissions;
using Orion.Models.ServerTransmissions;
using Orion.Models.ServerTransmissions.Results;

namespace Orion.Server.Users.Controllers
{
    public interface IUserController
    {
        public Task<ServerTransmission> AuthenticateUser(Credentials credentials);
    }
}
