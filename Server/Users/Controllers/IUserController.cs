using Orion.Models.ClientTransmissions;
using Orion.Models.ServerTransmissions.Results;

namespace Orion.Server.Users.Controllers
{
    public interface IUserController
    {
        public Task<ServerResult<AuthenticationMessages>?> AuthenticateUser(Credentials credentials);

        public Task<ServerResult?> AddDirectCommunication(Guid senderId, Guid recipientId);
    }
}
